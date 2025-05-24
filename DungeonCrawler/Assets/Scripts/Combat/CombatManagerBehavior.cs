using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


// Singleton
public class CombatManagerBehavior : MonoBehaviour
{
    private static CombatManagerBehavior instance = null;

    [SerializeField] List<GameObject> inputFriendlyCharacters = new List<GameObject>();
    [SerializeField] List<GameObject> inputEnemyCharacters = new List<GameObject>();
    [SerializeField] GameObject enemyContainerTemplate;
    [SerializeField] GameObject friendlyCharacterHolder;
    [SerializeField] GameObject enemyCharacterHolder;
    [SerializeField] int startingMana = 3;
    [SerializeField] int manaRegen = 2;
    [SerializeField] bool simulateTutorial = false;


    [HideInInspector] public static List<FriendlyBehavior> friendlyCharacterBehaviors = new List<FriendlyBehavior>();
    [HideInInspector] public static List<EnemyBehavior> enemyCharacterBehaviors = new List<EnemyBehavior>();
    
    private static float damageModifier = 1;
    private static FriendlySpellBehavior curSpellToCast = null;
    private static float clickBufferWait = .2f;
    private static float clickBufferTimer = 0f;
    private static GameObject youDiedScreen;

    [HideInInspector] public static bool inTutorial = false;
    [HideInInspector] public static bool inTutorialLevel = false;

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
        if (friendlyCharacterBehaviors.Count == 0)
        {
            foreach (GameObject character in instance.inputFriendlyCharacters)
            {
                character.SetActive(true);
                friendlyCharacterBehaviors.Add(character.GetComponent<FriendlyBehavior>());
            }
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

    public static bool loaded() { return instance != null; }

    public static List<FriendlyBehavior> getParty() { return friendlyCharacterBehaviors; }

    // Update is called once per frame
    void Update()
    {
        if (GameManagerBehavior.gameMode == E_GameMode.COMBAT)
        {
            clickBufferTimer += Time.deltaTime;
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
            character.gameObject.transform.parent.gameObject.SetActive(true);
            character.reset();
        }
        foreach (EnemyBehavior character in enemyCharacterBehaviors)
        {
            character.gameObject.transform.parent.gameObject.SetActive(true);
            character.reset();
        }
        curSpellToCast = null;
        StateManagerBehavior.reset();
        GameManagerBehavior.gameReset();
    }

    // called at the start of each combat
    public static void startBattle(CombatEncounterBehavior inputCombatData)
    {
        inTutorialLevel = false;
        inTutorial = false;
        battleSetUp(inputCombatData);
    }

    public static void startTutorial()
    {
        inTutorialLevel = true;
        inTutorial = true;
        battleSetUp(null);
        TutorialManagerBehavior.startTutorial();
    }

    private static void battleSetUp(CombatEncounterBehavior inputCombatData)
    {
        TeamManaBehavior.setManaWithoutEffect(instance.startingMana);
        clearOldEnemies();
        if (inputCombatData == null)
        {
            useDefaultEnemies();
        }
        else
        {
            createEnemies(inputCombatData);
        }

        foreach (FriendlyBehavior character in friendlyCharacterBehaviors)
        {
            character.gameObject.transform.parent.gameObject.SetActive(character.inTutorial || (!inTutorial));
            character.startBattle();
        }
        foreach (EnemyBehavior character in enemyCharacterBehaviors)
        {
            character.gameObject.transform.parent.gameObject.SetActive(character.inTutorial || (!inTutorial));
            character.startBattle();
        }
        

        PartySpellManagerBehavior.updateSpells(inTutorial);
        PartySpellManagerBehavior.UpdateSpellOrder();
        ArrowIndicatorManagerBehavior.createArrows(inTutorial);
        StateManagerBehavior.StartBattle();
    }

    private static void clearOldEnemies()
    {
        // reset so that you don't keep adding enemies
        enemyCharacterBehaviors.Clear();
        foreach (EnemyBehavior defaultEnemy in instance.enemyCharacterHolder.GetComponentsInChildren<EnemyBehavior>())
        {
            Destroy(defaultEnemy.gameObject.transform.parent.gameObject);
        }
    }

    private static void useDefaultEnemies()
    {
        // copy default enemies
        foreach (GameObject template in instance.inputEnemyCharacters)
        {
            GameObject copyContainer = Instantiate(template);
            RectTransform enemyContainerRect = copyContainer.GetComponent<RectTransform>();
            Vector3 scale = enemyContainerRect.localScale;
            copyContainer.transform.SetParent(instance.enemyCharacterHolder.transform);
            copyContainer.transform.localScale = scale;
            enemyCharacterBehaviors.Add(copyContainer.GetComponentInChildren<EnemyBehavior>());
        }
    }

    private static void createEnemies(CombatEncounterBehavior inputCombatData)
    {
        Debug.Log("create enemeies");

        // create new enemies
        foreach (EnemyStats curEnemyStat in inputCombatData.enemies)
        {
            GameObject enemyContainer = Instantiate<GameObject>(instance.enemyContainerTemplate);
            RectTransform enemyContainerRect = enemyContainer.GetComponent<RectTransform>();
            Vector3 scale = enemyContainerRect.localScale;
            enemyContainer.transform.SetParent(instance.enemyCharacterHolder.transform);
            enemyContainer.transform.localScale = scale;
            EnemyBehavior enemyBehavior = enemyContainer.GetComponentInChildren<EnemyBehavior>();
            enemyBehavior.setUpFromStats(curEnemyStat);
            enemyCharacterBehaviors.Add(enemyBehavior);
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
                instance.StartCoroutine(showYouDiedScreen());
                reset();
            }
            else
            {
                GameManagerBehavior.enterLevel();
            }
        }
    }

    public static void winCombatCheat()
    {
        endCombat(false);
    }

    private static IEnumerator showYouDiedScreen()
    {
		youDiedScreen.SetActive(true);
        yield return new WaitForSeconds(2);
        youDiedScreen.SetActive(false);
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
            character.updateHealth(-(int)(curSpellToCast.damage));
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
            target.updateHealth(-(int)(curSpellToCast.damage));
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
            else if (StateManagerBehavior.getState() == E_State.PLAYER_SPELL_SELECTION)
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

    public static bool getSimulateTutorial() { return instance.simulateTutorial; }

    public static int getManaRegen() { return instance.manaRegen; }
}

