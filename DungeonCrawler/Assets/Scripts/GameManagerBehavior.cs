using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManagerBehavior : MonoBehaviour
{
    StateManagerBehavior stateManager;
    public List<GameObject> friendlyCharacters = new List<GameObject>();
    public List<GameObject> enemyCharacters = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stateManager = FindFirstObjectByType<StateManagerBehavior>();
        stateManager.StartBattle(friendlyCharacters, enemyCharacters);
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
 
        if (canCast)
        {
            Debug.Log("cast " + spellBehavior.spellName + " " + spellBehavior.damage + " damage, " + spellBehavior.moraleDamage + " morale ");
            for (int i = 0; i < spellBehavior.castingCharacters.Count; i++)
            {
                CharacterBehavior curCharacter = behaviors[i];
                // do morale damage agains the casters
                curCharacter.cast(spellBehavior.moraleDamage);
            }
            //TODO attack enemies
        }
        else
        {
            Debug.Log("Failed to cast");
        }
    }

    // end turn
    public void endTurn()
    {
        if (stateManager.curState == E_State.PLAYER_SELECTION)
        {
            Debug.Log("end turn");
            stateManager.NextState();
        }
    }
}
