using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


// Singleton
public class GameManagerBehavior : MonoBehaviour
{
    public static GameManagerBehavior instance;

    [SerializeField] public List<GameObject> inputFriendlyCharacters = new List<GameObject>();
    [SerializeField] public List<GameObject> inputEnemyCharacters = new List<GameObject>();
    [SerializeField] public int startingMana = 10;

    public static List<GameObject> friendlyCharacters = new List<GameObject>();
    public static List<GameObject> enemyCharacters = new List<GameObject>();
    public static List<CharacterBehavior> friendlyCharacterBehaviors = new List<CharacterBehavior>();
    public static List<EnemyBehavior> enemyCharacterBehaviors = new List<EnemyBehavior>();

    private static int curMana;
    private static FriendlySpellBehavior curSpellToCast = null;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        foreach (GameObject character in inputFriendlyCharacters)
        {
            friendlyCharacters.Add(character);
            friendlyCharacterBehaviors.Add(character.GetComponent<CharacterBehavior>());
        }
        foreach (GameObject character in inputEnemyCharacters)
        {
            enemyCharacters.Add(character);
            enemyCharacterBehaviors.Add(character.GetComponent<EnemyBehavior>());
        }

    }

    private GameManagerBehavior() { }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StateManagerBehavior.StartBattle();
        curMana = startingMana;
    }

    // Update is called once per frame
    void Update()
    {
        checkCombatStatus();
        getInput();
    }

    private static void checkCombatStatus()
    {
        // check if all characters or all enemies are dead
        bool alive = false;
        foreach (CharacterBehavior character in GameManagerBehavior.friendlyCharacterBehaviors)
        {
            alive = alive | character.isActive();
        }
        if (alive == false)
        {
            endCombat();
        }

        alive = false;
        foreach (EnemyBehavior character in GameManagerBehavior.enemyCharacterBehaviors)
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
    public static void getInput()
    {
        E_State curState = StateManagerBehavior.getState();
        if (curState == E_State.PLAYER_SPELL_SELECTION || curState == E_State.PLAYER_ENEMY_TARGET_SELECTION)
        {

            if (Input.GetMouseButtonDown(0)) //left click
            {
                // get game objects
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.one, .5f);
                if (hit.collider != null)
                {
                    GameObject gameObject = hit.collider.gameObject;
                    if (gameObject.tag == "Enemy" && curState == E_State.PLAYER_ENEMY_TARGET_SELECTION)
                    {
                        castSpellOnTarget(gameObject);
                    }
                }


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
                            if (result.gameObject.tag == "Spell" && curState == E_State.PLAYER_SPELL_SELECTION)
                            {
                                FriendlySpellBehavior spellBehavior = result.gameObject.GetComponent<FriendlySpellBehavior>();
                                if (spellBehavior != null)
                                {
                                    resolveSpell(spellBehavior);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private static void endCombat()
    {
        // end combat
        // TODO create a game manager that holds level data, so that the scene isn't restarted
        SceneManager.LoadScene("Level1");
    }

    // takes a spell and determines if it can be cast
    // if it can, cast it or go to enemy selection state depending on the spell
    public static void resolveSpell(FriendlySpellBehavior spellBehavior)
    {
        bool canCast = true;
        List<CharacterBehavior> behaviors = new List<CharacterBehavior>();
        foreach (GameObject character in spellBehavior.castingCharacters)
        {
            CharacterBehavior curCharacter = character.GetComponent<CharacterBehavior>();
            behaviors.Add(curCharacter);
            canCast = canCast && curCharacter.canCast();
        }
 
        if (canCast && curMana - spellBehavior.manaCost >= 0)
        {
            curSpellToCast = (FriendlySpellBehavior)spellBehavior;
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
            DebugBehavior.updateLog("Failed to cast spell");
        }
    }

    // deal with mana and morale cost
    // to be used with other cast methods
    private static void cast()
    {
        curMana -= curSpellToCast.manaCost;

        foreach (GameObject character in curSpellToCast.castingCharacters)
        {
            CharacterBehavior curCharacter = character.GetComponent<CharacterBehavior>();
            // do morale damage against the casters
            curCharacter.cast(curSpellToCast.moraleDamage);
        }
    }

    // casts the stored spell selected by the player on all enemies
    public static void castSpellOnAll()
    {
        DebugBehavior.updateLog("cast " + curSpellToCast.spellName + " " + curSpellToCast.damage + " damage, " + curSpellToCast.moraleDamage + " morale, " + curSpellToCast.manaCost + " mana on all enemies");
        cast();

        foreach (EnemyBehavior character in enemyCharacterBehaviors)
        {
            character.updateHealth(-curSpellToCast.damage);
        }
        StateManagerBehavior.NextState(E_State.PLAYER_SPELL_SELECTION);
    }

    // casts the stored spell selected by the player on the enemy selected by the player
    public static void castSpellOnTarget(GameObject selectedEnemy)
    {
        DebugBehavior.updateLog("cast " + curSpellToCast.spellName + " " + curSpellToCast.damage + " damage, " + curSpellToCast.moraleDamage + " morale, " + curSpellToCast.manaCost + " mana on selected enemy");
        cast();

        selectedEnemy.GetComponent<EnemyBehavior>().updateHealth(-curSpellToCast.damage);
        StateManagerBehavior.NextState(E_State.PLAYER_SPELL_SELECTION);
    }

    public static void playerStartTurn()
    {
        // regen energy
        curMana += 2;
        // reset all friendly characters
        foreach (CharacterBehavior character in friendlyCharacterBehaviors)
        {
            character.startTurn();
        }
    }
    public static void playerEndTurn()
    {
        if (StateManagerBehavior.getState() == E_State.PLAYER_SPELL_SELECTION)
        {
            StateManagerBehavior.NextState(E_State.ENEMY_BUFFER);
        }
    }

    public static int getMana() { return curMana; }
}
