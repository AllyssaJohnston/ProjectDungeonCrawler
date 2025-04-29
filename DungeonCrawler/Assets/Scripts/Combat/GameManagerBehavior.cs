using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManagerBehavior : MonoBehaviour
{
    StateManagerBehavior stateManager;
    public List<GameObject> friendlyCharacters = new List<GameObject>();
    public List<GameObject> enemyCharacters = new List<GameObject>();
    public int startingMana = 10;
    private int curMana;
    FriendlySpellBehavior curSpellToCast = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stateManager = FindFirstObjectByType<StateManagerBehavior>();
        // TODO startbattle should be called from the level
        // This is here for debugging/ ease of testing
        stateManager.StartBattle(friendlyCharacters, enemyCharacters);

        curMana = startingMana;
    }

    // Update is called once per frame
    void Update()
    {
        getInput();
    }

    public void getInput()
    {
        if (stateManager.curState == E_State.PLAYER_SPELL_SELECTION || stateManager.curState == E_State.PLAYER_ENEMY_TARGET_SELECTION)
        {

            if (Input.GetMouseButtonDown(0)) //left click
            {
                //get game objects
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.one, .5f);
                if (hit.collider != null)
                {
                    Debug.Log("hit something");
                    GameObject gameObject = hit.collider.gameObject;
                    if (gameObject.tag == "Enemy" && stateManager.curState == E_State.PLAYER_ENEMY_TARGET_SELECTION)
                    {
                        Debug.Log("hit enemy");
                        castSpellOnTarget(gameObject);
                    }
                }


                //find UI elements
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
                            if (result.gameObject.tag == "Spell" && stateManager.curState == E_State.PLAYER_SPELL_SELECTION)
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

    // takes a spell and determines if it can be cast
    // if it can, will go to enemy selection state
    public void resolveSpell(FriendlySpellBehavior spellBehavior)
    {
        bool canCast = true;
        List<CharacterBehavior> behaviors = new List<CharacterBehavior>();
        for (int i = 0; i < spellBehavior.castingCharacters.Count; i++)
        {
            CharacterBehavior curCharacter = spellBehavior.castingCharacters[i].GetComponent<CharacterBehavior>();
            behaviors.Add(curCharacter);
            canCast = canCast && curCharacter.canCast();
        }
 
        if (canCast && curMana - spellBehavior.manaCost >= 0)
        {
            curSpellToCast = (FriendlySpellBehavior)spellBehavior;
            if (spellBehavior.damageAllEnemies == false)
            {
                // go to target selection state
                stateManager.NextState(); // select enemy state
            }
            else
            {
                // cast spell on all enemies
                castSpellOnAll();
            }
        }
        else
        {
            Debug.Log("Failed to cast");
        }
    }

    // deal with mana and morale cost
    // to be used with other cast methods
    private void cast()
    {
        curMana -= curSpellToCast.manaCost;

        for (int i = 0; i < curSpellToCast.castingCharacters.Count; i++)
        {
            CharacterBehavior curCharacter = curSpellToCast.castingCharacters[i].GetComponent<CharacterBehavior>();
            // do morale damage against the casters
            curCharacter.cast(curSpellToCast.moraleDamage);
        }
    }

    // casts the stored spell selected by the player on all enemies
    public void castSpellOnAll()
    {
        Debug.Log("cast " + curSpellToCast.spellName + " " + curSpellToCast.damage + " damage, " + curSpellToCast.moraleDamage + " morale " + curSpellToCast.manaCost + " mana on all enemies");
        cast();

        for (int i = 0; i < enemyCharacters.Count; i++)
        {
            enemyCharacters[i].GetComponent<EnemyBehavior>().updateHealth(-curSpellToCast.damage);
        }
        stateManager.NextState(E_State.PLAYER_SPELL_SELECTION);
    }

    // casts the stored spell selected by the player on the enemy selected by the player
    public void castSpellOnTarget(GameObject selectedEnemy)
    {
        Debug.Log("cast " + curSpellToCast.spellName + " " + curSpellToCast.damage + " damage, " + curSpellToCast.moraleDamage + " morale " + curSpellToCast.manaCost + " mana on selected enemy");
        cast();

        selectedEnemy.GetComponent<EnemyBehavior>().updateHealth(-curSpellToCast.damage);
        stateManager.NextState(E_State.PLAYER_SPELL_SELECTION);
    }

    public void playerStartTurn()
    {
        // regen energy
        curMana += 2;
    }
    public void playerEndTurn()
    {
        if (stateManager.curState == E_State.PLAYER_SPELL_SELECTION)
        {
            Debug.Log("end turn");
            stateManager.NextState(E_State.ENEMY_ACTION);
        }
    }

    public int getMana()
    {
        return curMana;
    }
}
