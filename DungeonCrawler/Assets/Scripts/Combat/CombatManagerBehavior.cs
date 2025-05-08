using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


// Singleton
public class CombatManagerBehavior : MonoBehaviour
{
    private static CombatManagerBehavior instance;

    // These are here so that you can edit the characters in editor
    public List<GameObject> inputFriendlyCharacters = new List<GameObject>();
    public List<GameObject> inputEnemyCharacters = new List<GameObject>();
    [SerializeField] GameObject enemyContainerTemplate;
    [SerializeField] GameObject characterHolder;
    [SerializeField] private int startingMana = 3;
    [SerializeField] private int manaRegen = 2;

    // These are the fields we actually want to work with
    [HideInInspector] public static List<CharacterBehavior> friendlyCharacterBehaviors = new List<CharacterBehavior>();
    [HideInInspector] public static List<EnemyBehavior> enemyCharacterBehaviors = new List<EnemyBehavior>();

    private static FriendlySpellBehavior curSpellToCast = null;

    public static bool combatStarted {get; private set;}

    public void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        Debug.Log("combat manager initialized");
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private CombatManagerBehavior() { }

    public static bool loaded() {
        return instance != null;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManagerBehavior.gameMode == E_GameMode.COMBAT)
        {
            checkCombatStatus();
            getInput();
        }
    }

    private static void checkCombatStatus()
    {
        // check if all characters or all enemies are dead
        bool alive = false;
        foreach (CharacterBehavior character in friendlyCharacterBehaviors)
        {
            alive = alive | character.isActive();
        }
        if (alive == false)
        {
            endCombat();
        }

        alive = false;
        foreach (EnemyBehavior character in enemyCharacterBehaviors)
        {
            alive = alive | character.isActive();
        }
        if (alive == false)
        {
            endCombat();
        }
    }

    // gets input and uses it
    // TODO separate get and use input
    private static void getInput()
    {
        E_State curState = StateManagerBehavior.getState();
        if (Input.GetMouseButtonDown(0)) //left click
        {
            // get game objects
            //if (curState == E_State.PLAYER_ENEMY_TARGET_SELECTION)
            //{
            //    Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //    RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.one, .5f);
            //    if (hit.collider != null)
            //    {
            //        GameObject gameObject = hit.collider.gameObject;
            //        if (gameObject.tag == "Enemy")
            //        {
            //            castSpellOnTarget(gameObject.GetComponent<EnemyBehavior>());
            //        }
            //    }
            //}

            // find UI elements
            if (EventSystem.current.IsPointerOverGameObject())
            {
                PointerEventData pointer = new PointerEventData(EventSystem.current);
                pointer.position = Input.mousePosition;

                List<RaycastResult> raycastResults = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointer, raycastResults);

                if (raycastResults.Count > 0)
                {
                    foreach (RaycastResult result in raycastResults)
                    {
                        if (result.gameObject.tag == "Spell")
                        {
                            if (curState == E_State.PLAYER_SPELL_SELECTION)
                            {
                                // good
                            }
                            else if (curState == E_State.PLAYER_BETWEEN_SPELLS_BUFFFER || curState == E_State.ENEMY_END_TURN_BUFFER)
                            {
                                // interupt state
                                StateManagerBehavior.InteruptState();
                                StateManagerBehavior.NextState(E_State.PLAYER_SPELL_SELECTION);
                            }
                            else
                            {
                                // no
                                return;
                            }

                            FriendlySpellBehavior spellBehavior = result.gameObject.GetComponent<FriendlySpellBehavior>();
                            if (spellBehavior != null)
                            {
                                resolveSpell(spellBehavior);
                            }
                        }
                        else if (result.gameObject.tag == "Enemy")
                        {
                            if (curState == E_State.PLAYER_ENEMY_TARGET_SELECTION)
                            {
                                Debug.Log("hit enemy");
                                castSpellOnTarget(result.gameObject.GetComponent<EnemyBehavior>());
                            }
                        }
                    }
                }
            }
        }
    }
    
    // called at the start of each combat
    public static void startBattle(CombatEncounterBehavior inputCombatData)
    {
        if (GameManagerBehavior.gameMode == E_GameMode.COMBAT)
        {
            if (inputCombatData == null)
            {
                battleSetUp();
                useDefaultEnemies();
            }
            else
            {
                battleSetUp();
                createEnemies(inputCombatData);
            }
        }
        combatStarted = true;
    }


    private static void battleSetUp()
    {
        TeamManaBehavior.setManaWithoutEffect(instance.startingMana);
        friendlyCharacterBehaviors.Clear();
        foreach (GameObject character in instance.inputFriendlyCharacters)
        {
            friendlyCharacterBehaviors.Add(character.GetComponent<CharacterBehavior>());
            character.GetComponent<CharacterBehavior>().startBattle();
        }
        PartySpellManagerBehavior.UpdateSpellOrder();
        StateManagerBehavior.StartBattle();
    }

    private static void useDefaultEnemies()
    {
        // reset so that you can enter the same encounter multiple times
        enemyCharacterBehaviors.Clear();
        foreach (GameObject character in instance.inputEnemyCharacters)
        {
            enemyCharacterBehaviors.Add(character.GetComponent<EnemyBehavior>());
            character.GetComponent<CharacterBehavior>().startBattle();
        }
    }

    private static void createEnemies(CombatEncounterBehavior inputCombatData)
    {
        float screenX = 300;
        float yPos = 6.1f;
        float spacing = 130f;

        
        int i = inputCombatData.enemies.Count - 1;
        //clear old enemies
        foreach (GameObject defaultEnemy in instance.inputEnemyCharacters)
        {
            Destroy(defaultEnemy);
        }
        enemyCharacterBehaviors.Clear();
        instance.inputEnemyCharacters.Clear();

        //create new enemies
        foreach (EnemyStats curEnemyStat in inputCombatData.enemies)
        {
            GameObject enemyContainer = Instantiate<GameObject>(instance.enemyContainerTemplate);
            GameObject enemy = enemyContainer.transform.GetChild(0).gameObject; // only one child of enemy container
            RectTransform enemyContainerRect = enemyContainer.GetComponent<RectTransform>();
            enemyContainer.transform.SetParent(instance.characterHolder.transform);
            enemyContainerRect.anchoredPosition = new Vector3(screenX - i * (spacing), yPos, 0);
            foreach (Behaviour component in enemy.GetComponents<Behaviour>())
            {
                component.enabled = true;
            }
            EnemyBehavior enemyBehavior = enemy.GetComponent<EnemyBehavior>();
            enemyBehavior.setUpFromStats(curEnemyStat);
            enemyCharacterBehaviors.Add(enemyBehavior);
            enemyBehavior.startBattle();
            i--;
        }
    }

    // called to end combat
    private static void endCombat()
    {
        if (GameManagerBehavior.gameMode == E_GameMode.COMBAT)
        {
            Debug.Log("end combat");
            GameManagerBehavior.enterLevel();
        }
    }

    public static void OnNextState(E_State nextState)
    {
        switch (nextState)
        {
            case E_State.ENEMY_END_TURN_BUFFER:
                playerStartTurn();
                break;
        }
    }

    // takes a spell and determines if it can be cast
    // if it can, cast it or go to enemy selection state depending on the spell
    private static void resolveSpell(FriendlySpellBehavior spellBehavior)
    {
        // make sure everyone is available to cast
        bool canCast = true;
        foreach (CharacterBehavior character in spellBehavior.castingCharacterBehaviors)
        {
            canCast = canCast && character.canCast();
        }
 
        // make sure there is enough mana
        if (canCast && TeamManaBehavior.getMana() - spellBehavior.manaCost >= 0)
        {
            curSpellToCast = spellBehavior;
            if (spellBehavior.damageAllEnemies)
            {
                // cast spell on all enemies
                castSpellOnAll();
            }
            else
            {
                // go to target selection state
                StateManagerBehavior.NextState(); // select enemy state
            }
        }
        else
        {
            DebugBehavior.updateLog("Failed to cast spell.");
        }
    }
    
    // for player casted spells
    // deal with mana and morale cost
    // to be used with other cast methods
    private static void cast()
    {
        TeamManaBehavior.updateMana(-curSpellToCast.manaCost);

        foreach (CharacterBehavior character in curSpellToCast.castingCharacterBehaviors)
        {
            // do morale damage against the casters
            character.cast(curSpellToCast.moraleDamage);
        }
        PartySpellManagerBehavior.UpdateSpellOrder();
        StateManagerBehavior.NextState(E_State.PLAYER_BETWEEN_SPELLS_BUFFFER);
    }

    // for player casted spells
    // casts the stored spell selected by the player on all enemies
    private static void castSpellOnAll()
    {
        DebugBehavior.updateLog("Cast " + curSpellToCast.spellName + " for " + curSpellToCast.damage + " damage on all enemies, costing " + curSpellToCast.manaCost + " mana and " + curSpellToCast.moraleDamage + " morale.");
        foreach (EnemyBehavior character in enemyCharacterBehaviors)
        {
            character.updateHealth(-curSpellToCast.damage);
        }
        cast();
    }

    // for player casted spells
    // casts the stored spell selected by the player on the enemy selected by the player
    private static void castSpellOnTarget(EnemyBehavior selectedEnemy)
    {
        DebugBehavior.updateLog("Cast " + curSpellToCast.spellName + " for " + curSpellToCast.damage + " damage on " + selectedEnemy.characterName + ", costing " + curSpellToCast.manaCost + " mana and " + curSpellToCast.moraleDamage + " morale.");
        selectedEnemy.updateHealth(-curSpellToCast.damage);
        cast();
    }

    // called at the start of the player turn
    public static void playerStartTurn()
    {
        // regen mana
        TeamManaBehavior.updateMana(instance.manaRegen);

        // reset all friendly characters
        foreach (CharacterBehavior character in friendlyCharacterBehaviors)
        {
            character.startTurn();
        }
        PartySpellManagerBehavior.UpdateSpellOrder();
    }

    // called at the end of the player turn (button click)
    public static void playerEndTurn()
    {
        if (StateManagerBehavior.getState() == E_State.PLAYER_BETWEEN_SPELLS_BUFFFER || StateManagerBehavior.getState() == E_State.ENEMY_END_TURN_BUFFER)
        {
            StateManagerBehavior.InteruptState();
            StateManagerBehavior.NextState(E_State.PLAYER_SPELL_SELECTION);
        }
        if (StateManagerBehavior.getState() == E_State.PLAYER_SPELL_SELECTION)
        {
            StateManagerBehavior.NextState(E_State.PLAYER_END_TURN_BUFFER);
        }
    }

    // called at the start of the enemies' turn
    public static void enemiesStartTurn()
    {
        foreach (EnemyBehavior character in enemyCharacterBehaviors)
        {
            character.startTurn();
        }
    }
}
