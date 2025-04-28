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
        if (stateManager.curState == E_State.PLAYER_SELECTION)
        {

            if (Input.GetMouseButtonDown(0)) //left click
            {
                // get game objects
                //Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.one, .5f);
                //if (hit.collider != null)
                //{
                //    GameObject gameObject = hit.collider.gameObject;
                //    Debug.Log("found something");
                //}


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
                            if (result.gameObject.tag == "Spell")
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

    // takes a spell and tries to cast it if possible
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
            Debug.Log("cast " + spellBehavior.spellName + " " + spellBehavior.damage + " damage, " + spellBehavior.moraleDamage + " morale " + spellBehavior.manaCost + " mana ");
            curMana -= spellBehavior.manaCost;
            for (int i = 0; i < spellBehavior.castingCharacters.Count; i++)
            {
                CharacterBehavior curCharacter = behaviors[i];
                // do morale damage agains the casters
                curCharacter.cast(spellBehavior.moraleDamage);
            }

            //TODO update attack enemies
            EnemyBehavior enemy = enemyCharacters[0].GetComponent<EnemyBehavior>();
            enemy.updateHealth(-spellBehavior.damage);
        }
        else
        {
            Debug.Log("Failed to cast");
        }
    }

    public void playerStartTurn()
    {
        // regen energy
        curMana += 2;
    }


    public void playerEndTurn()
    {
        if (stateManager.curState == E_State.PLAYER_SELECTION)
        {
            Debug.Log("end turn");
            stateManager.NextState();
        }
    }

    public int getMana()
    {
        return curMana;
    }
}
