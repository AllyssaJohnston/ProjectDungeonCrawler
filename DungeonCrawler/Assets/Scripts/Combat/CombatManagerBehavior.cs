using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


// Singleton
public class CombatManagerBehavior : MonoBehaviour
{
    [HideInInspector] public static CombatManagerBehavior instance = null;

    public List<GameObject> inputFriendlyCharacters = new List<GameObject>();
    public List<GameObject> inputEnemyCharacters = new List<GameObject>();
    [SerializeField] GameObject enemyContainerTemplate;
    [SerializeField] GameObject characterHolder;
    [SerializeField] private int startingMana = 3;
    [SerializeField] private int manaRegen = 2;


    [HideInInspector] public static List<FriendlyBehavior> friendlyCharacterBehaviors = new List<FriendlyBehavior>();
    [HideInInspector] public static List<EnemyBehavior> enemyCharacterBehaviors = new List<EnemyBehavior>();
    private static float damageModifier = 1;

    private static FriendlySpellBehavior curSpellToCast = null;
    public static bool combatStarted { get; private set; }
    public static bool inTutorial = false;

    private static float clickBufferWait = .2f;
    private static float clickBufferTimer = 0f;

    private static GameObject youDiedScreen;

	public void Start()
    {
        if (instance != null && instance != this)
        {
            // destroy all new copies
            Destroy(gameObject);
            return;
        }

        instance = this;
        Debug.Log("combat manager initialized");
		// will get called each reload from the main menu
		friendlyCharacterBehaviors.Clear();
        foreach (GameObject character in instance.inputFriendlyCharacters)
        {
            friendlyCharacterBehaviors.Add(character.GetComponent<FriendlyBehavior>());
        }
        youDiedScreen = GameObject.FindWithTag("YouDead");
        youDiedScreen.SetActive(false);
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        return;
    }

    private CombatManagerBehavior() { }

    public static bool loaded()
    {
        return instance != null;
    }

    public static List<GameObject> getParty()
    {
        if (instance == null) return null;
        else return instance.inputFriendlyCharacters;
    }

    // Update is called once per frame
    void Update()
    {
        clickBufferTimer += Time.deltaTime;
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
        foreach (FriendlyBehavior character in friendlyCharacterBehaviors)
        {
            alive = alive | character.isAlive();
        }
        if (alive == false)
        {
            endCombat(true);
        }

        alive = false;
        foreach (EnemyBehavior character in enemyCharacterBehaviors)
        {
            alive = alive | character.isAlive();
        }
        if (alive == false)
        {
            endCombat(false);
        }
    }

    // gets input and uses it
    private static void getInput()
    {
        E_State curState = StateManagerBehavior.getState();
        if (Input.GetMouseButtonDown(0) && clickBufferTimer >= clickBufferWait) //left click
        {
            clickBufferTimer = 0f;
            // find UI elements
            if (EventSystem.current.IsPointerOverGameObject())
            {
                PointerEventData pointer = new PointerEventData(EventSystem.current);
                pointer.position = Input.mousePosition;

                List<RaycastResult> raycastResults = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointer, raycastResults);

                if (raycastResults.Count > 0)
                {
                    GameManagerBehavior.pop();
					foreach (RaycastResult result in raycastResults)
                    {
                        if (result.gameObject.tag == "Spell")
                        {
                            if (curState == E_State.PLAYER_SPELL_SELECTION)
                            {
                                // good
                            }
                            else if (curState == E_State.PLAYER_ENEMY_TARGET_SELECTION || curState == E_State.PLAYER_FRIENDLY_TARGET_SELECTION)
                            {
                                // allow spell reselection

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
                            if (spellBehavior != null && (!inTutorial || (inTutorial && TutorialManagerBehavior.isValidSpell(spellBehavior))))
                            {
                                resolveSpell(spellBehavior);
                                return;
                            }
                        }
                        else if (result.gameObject.tag == "Enemy")
                        {
                            if (curState == E_State.PLAYER_ENEMY_TARGET_SELECTION)
                            {
                                if (!inTutorial || (inTutorial && TutorialManagerBehavior.canSelectEnemy()))
                                {
                                    friendlyCastSpellOnTargetEnemy(result.gameObject.GetComponent<EnemyBehavior>());
                                    return;
                                }
                            }
                        }
                        else if (result.gameObject.tag == "Friendly")
                        {
                            if (curState == E_State.PLAYER_FRIENDLY_TARGET_SELECTION)
                            {
                                friendlyCastSpellOnTargetFriendly(result.gameObject.GetComponent<FriendlyBehavior>());
                                return;
                            }
                        }
                        else if (result.gameObject.tag == "TutorialPanel")
                        {
                            TutorialManagerBehavior.panelClicked();
                            return;
                        }
                    }
                }
            }
        }
    }

    // reset all stats
    public static void reset()
    {
        Debug.Log("resetting combat");
        foreach (FriendlyBehavior character in friendlyCharacterBehaviors)
        {
            character.reset();
        }
        foreach (EnemyBehavior character in enemyCharacterBehaviors)
        {
            character.reset();
        }
    }

    // called at the start of each combat
    public static void startBattle(CombatEncounterBehavior inputCombatData)
    {
        if (GameManagerBehavior.gameMode == E_GameMode.COMBAT)
        {
            TeamManaBehavior.setManaWithoutEffect(instance.startingMana);
            if (inputCombatData == null)
            {
                useDefaultEnemies();
            }
            else
            {
                createEnemies(inputCombatData);
            }
            battleSetUp();
			combatStarted = true;
        }
    }

    private static void battleSetUp()
    {
        foreach (FriendlyBehavior character in friendlyCharacterBehaviors)
        {
            character.startBattle();
        }
        PartySpellManagerBehavior.updateSpells();
        PartySpellManagerBehavior.UpdateSpellOrder();
        if (!inTutorial)
        {
            ArrowIndicatorManagerBehavior.createArrows();
        }
        StateManagerBehavior.StartBattle();
    }

    private static void useDefaultEnemies()
    {
        // reset so that you can enter the same encounter multiple times
        enemyCharacterBehaviors.Clear();
        foreach (GameObject character in instance.inputEnemyCharacters)
        {
            EnemyBehavior behavior = character.GetComponent<EnemyBehavior>();
            enemyCharacterBehaviors.Add(behavior);
            behavior.startBattle();
        }
    }

    private static void createEnemies(CombatEncounterBehavior inputCombatData)
    {
        Debug.Log("create enemeies");
        float screenX = instance.characterHolder.GetComponent<RectTransform>().offsetMax.x - 295;
        float yPos = friendlyCharacterBehaviors[0].gameObject.transform.parent.localPosition.y;
        float spacing = 70f;


        int i = inputCombatData.enemies.Count - 1;
        //clear old enemies
        foreach (GameObject defaultEnemy in instance.inputEnemyCharacters)
        {
            Destroy(defaultEnemy.transform.parent.gameObject);
        }
        enemyCharacterBehaviors.Clear();
        instance.inputEnemyCharacters.Clear();

        //create new enemies
        foreach (EnemyStats curEnemyStat in inputCombatData.enemies)
        {
            GameObject enemyContainer = Instantiate<GameObject>(instance.enemyContainerTemplate);
            GameObject enemy = enemyContainer.transform.GetChild(0).gameObject; // only one child of enemy container
            RectTransform enemyContainerRect = enemyContainer.GetComponent<RectTransform>();
            Vector3 scale = enemyContainerRect.localScale;
            Vector3 pos = enemyContainerRect.position;
            Debug.Log(pos);
            enemyContainer.transform.SetParent(instance.characterHolder.transform);
            enemyContainer.transform.localScale = scale;
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
    private static void endCombat(bool died)
    {
		Debug.Log("end combat");
        if (GameManagerBehavior.gameMode == E_GameMode.COMBAT)
        {
            if (died)
            {
                instance.StartCoroutine(exitToMainMenu());
            }
            else
            {
                GameManagerBehavior.enterLevel();
            }
        }
    }

    private static IEnumerator exitToMainMenu()
    {
		youDiedScreen.SetActive(true);
        yield return new WaitForSeconds(2);
        youDiedScreen.SetActive(false);
        // go to main menu
        reset();
        SceneManager.LoadScene("Menu");
        GameManagerBehavior.enterLevel();
    }

    public static void OnNextState(E_State oldState, E_State nextState)
    {
        if (oldState == E_State.ENEMY_END_TURN_BUFFER && nextState == E_State.PLAYER_SPELL_SELECTION)
        {
            playerStartTurn();
            enemiesStartTurn();
        }
    }

    public static void updateDamageModifier(float givenDamageModifier)
    {
        damageModifier = givenDamageModifier;
        PartySpellManagerBehavior.UpdateSpellTextStats(damageModifier);
    }

    // takes a spell and determines if it can be cast
    // if it can, cast it or go to enemy selection state depending on the spell
    private static void resolveSpell(FriendlySpellBehavior spellBehavior)
    {
        // make sure everyone is available to cast
        bool canCast = true;
        foreach (FriendlyBehavior character in spellBehavior.castingCharacterBehaviors)
        {
            canCast = canCast && character.canCast();
        }

        // make sure there is enough mana
        if (canCast && TeamManaBehavior.getMana() - spellBehavior.manaCost >= 0)
        {
            curSpellToCast = spellBehavior;
            switch (curSpellToCast.targeting)
            {
                case (E_SPELL_TARGETING.ALL_ENEMIES):
                    friendlyCastSpellOnAllEnemies();
                    break;
                case (E_SPELL_TARGETING.SINGLE_ENEMY):
                    StateManagerBehavior.NextState(E_State.PLAYER_ENEMY_TARGET_SELECTION);
                    break;
                case (E_SPELL_TARGETING.ALL_FRIENDLIES):
                    friendlyCastSpellAllFriendlies();
                    break;
                case (E_SPELL_TARGETING.SINGLE_FRIENDLY):
                    StateManagerBehavior.NextState(E_State.PLAYER_FRIENDLY_TARGET_SELECTION);
                    break;
                default:
                    Debug.Log("Unrecognzied spell targeting");
                    break;
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
    private static void friendlyCast()
    {
        TeamManaBehavior.updateMana(-curSpellToCast.manaCost);

        foreach (FriendlyBehavior character in curSpellToCast.castingCharacterBehaviors)
        {
            character.cast(-curSpellToCast.moraleDamageToSelf); // do morale damage against the casters
        }
        if (curSpellToCast.moraleRegen > 0 || curSpellToCast.heal > 0)
        {
            foreach (FriendlyBehavior character in friendlyCharacterBehaviors)
            {
                character.updateHealth(curSpellToCast.heal); // regen health
                character.updateMorale(curSpellToCast.moraleRegen); // regen morale
            }
        }
        TeamManaBehavior.updateMana(curSpellToCast.manaRegen); // regen mana

        PartySpellManagerBehavior.UpdateSpellOrder();
        StateManagerBehavior.NextState(E_State.PLAYER_BETWEEN_SPELLS_BUFFFER);
    }

    // for player casted spells
    // casts the stored spell selected by the player on all enemies
    private static void friendlyCastSpellOnAllEnemies()
    {
        DebugBehavior.updateLog(curSpellToCast.castingCharactersText + " cast " + curSpellToCast.spellDescriptionText + " on all enemies");
        foreach (EnemyBehavior character in enemyCharacterBehaviors)
        {
            character.updateHealth(-(int)(curSpellToCast.damage * damageModifier));
        }
        friendlyCast();
    }

    // for player casted spells
    // casts the stored spell selected by the player on the enemy selected by the player
    private static void friendlyCastSpellOnTargetEnemy(EnemyBehavior target)
    {
        if (target.isAlive()) //dont cast on dead targets
        {
            DebugBehavior.updateLog(curSpellToCast.castingCharactersText + " cast " + curSpellToCast.spellDescriptionText + " on " + target.characterName);
            target.updateHealth(-(int)(curSpellToCast.damage * damageModifier));
            if (curSpellToCast.stun)
            {
                target.stun();
            }
            friendlyCast();
        }
        else
        {
            DebugBehavior.updateLog("Failed to cast spell.");
        }
    }

    // for player casted spells
    // casts the stored spell selected by the player on the friendly selected by the player
    private static void friendlyCastSpellOnTargetFriendly(FriendlyBehavior target)
    {
        if (curSpellToCast.revive && !target.isAlive()) // only cast revive on dead targets
        {
            DebugBehavior.updateLog(curSpellToCast.castingCharactersText + " cast " + curSpellToCast.spellDescriptionText + " on " + target.characterName);
            if (curSpellToCast.revive)
            {
                target.revive();
            }
            friendlyCast();
        }
        else
        {
            DebugBehavior.updateLog("Failed to cast spell.");
        }
    }

    // for player casted spells
    // casts the stored spell selected by the player on the enemy selected by the player
    private static void friendlyCastSpellAllFriendlies()
    {
        DebugBehavior.updateLog(curSpellToCast.castingCharactersText + " cast " + curSpellToCast.spellDescriptionText + " on all party members.");
        friendlyCast();
    }



    //chooses a spell and executes it
    public static void enemyCastSpell()
    {
        EnemyBehavior curEnemy = enemyCharacterBehaviors[StateManagerBehavior.curEnemyIndex];
        if (curEnemy.canCast())
        {
            EnemySpellStats spell = curEnemy.getSpell();

            string target = "all party members";
            if (spell.damageAllEnemies)
            {
                foreach (FriendlyBehavior characterBehavior in friendlyCharacterBehaviors)
                {
                    if (characterBehavior.gameObject.transform.parent.gameObject.activeSelf) // check if parent container is active (for tutorial level)
                    {
                        characterBehavior.updateHealth(-spell.damage);
                        characterBehavior.updateMorale(-spell.moraleDamageToEnemies);
                    }
                }
            }
            else
            {
                //select character to target based on spell targeting data, and attack them
                FriendlyBehavior characterBehavior = curEnemy.getCharacterTarget(spell.characterTargeting, friendlyCharacterBehaviors);
                characterBehavior.updateHealth(-spell.damage);
                characterBehavior.updateMorale(-spell.moraleDamageToEnemies);
                target = characterBehavior.characterName;
            }
            curEnemy.updateHealth(spell.heal); // heal enemy
            DebugBehavior.updateLog(curEnemy.characterName + " cast " + spell.spellDescriptionText + " on " + target);
            curEnemy.cast();
        }
        else
        {
            DebugBehavior.updateLog(curEnemy.characterName + " can't cast!");
        }
    }

    // called at the start of the player turn
    public static void playerStartTurn()
    {
        // regen mana
        TeamManaBehavior.updateMana(instance.manaRegen);

        // reset all friendly characters
        foreach (FriendlyBehavior character in friendlyCharacterBehaviors)
        {
            character.startTurn();
        }
        PartySpellManagerBehavior.UpdateSpellOrder();
    }

    // called at the end of the player turn (button click)
    public static void playerEndTurn()
    {
        if (!inTutorial || (inTutorial && TutorialManagerBehavior.canEndTurn()))
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
    }

    // called at the start of the enemies' turn
    public static void enemiesStartTurn()
    {
        foreach (EnemyBehavior character in enemyCharacterBehaviors)
        {
            character.startTurn();
        }
    }

    public static int getManaRegen()
    {
        return instance.manaRegen;
    }

}

