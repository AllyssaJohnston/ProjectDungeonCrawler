using System.Collections.Generic;
using UnityEngine;

public enum E_State
{
    PLAYER_SELECTION,
    ENEMY_ACTION
}

public class StateManagerBehavior : MonoBehaviour
{
    public E_State curState = E_State.PLAYER_SELECTION;
    List<GameObject> friendlyCharacters = new List<GameObject>();
    List<GameObject> enemyCharacters = new List<GameObject>();
    List<CharacterBehavior> friendlyBehaviors = new List<CharacterBehavior>();
    List<EnemyBehavior> enemyBehaviors = new List<EnemyBehavior>();
    public GameObject characterStatObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (curState)
        {
            case E_State.PLAYER_SELECTION:
                // do nothing, player needs to do things to end this state
                break;
            case E_State.ENEMY_ACTION:
                for (int i = 0; i < enemyBehaviors.Count; i++)
                {
                    enemyBehaviors[i].startTurn();
                }
                for (int i = 0; i < enemyBehaviors.Count; i++) //this could be simplified if we don't need all enemies to reset before each enemy casts a spell
                {
                    enemyBehaviors[i].chooseSpell(friendlyBehaviors);
                }
                NextState();

                //lazy reset
                for (int i = 0; i < friendlyBehaviors.Count; i++)
                {
                    friendlyBehaviors[i].startTurn();
                }
                break;
            default:
                Debug.Log("Invalid state" + curState);
                break;
        }
    }


    public void StartBattle(List<GameObject> givenFriendlyCharacters, List<GameObject> givenEnemyCharacters)
    {
        friendlyCharacters = givenFriendlyCharacters;
        enemyCharacters = givenEnemyCharacters;
        for (int i = 0; i < friendlyCharacters.Count; i++)
        {
            //character
            CharacterBehavior character = friendlyCharacters[i].GetComponent<CharacterBehavior>();
            friendlyBehaviors.Add(character);
            character.startBattle();

            //TODO create character stats, which will show character health and morale
            // need to find a way to show this stat above the character
            ////character stat
            //GameObject characterStat = Instantiate(characterStatObject);
            ////characterStat.transform.position = friendlyCharacters[i].transform.position + new Vector3(0, 20, 0);
            //CharacterStatBehavior characterStatBehavior = characterStat.GetComponent<CharacterStatBehavior>();
            //characterStatBehavior.character = character;

        }
        for (int i = 0; i < enemyCharacters.Count; i++)
        {
            EnemyBehavior enemy = enemyCharacters[i].GetComponent<EnemyBehavior>();
            enemyBehaviors.Add(enemy);
            enemy.startBattle(); 
        }
    }

    // go to the given state
    public void NextState(E_State nextState)
    {
        curState = nextState;
        Debug.Log("Curstate = " + curState);
    }

    // go to the next state
    public void NextState()
    {
        switch(curState)
        {
            case E_State.PLAYER_SELECTION:
                // go to enmey state
                curState = E_State.ENEMY_ACTION;
                break;
            case E_State.ENEMY_ACTION:
                // go to player state 
                curState = E_State.PLAYER_SELECTION;
                for (int i = 0; i < friendlyBehaviors.Count; i++)
                {
                    friendlyBehaviors[i].startTurn();
                }
                break;
            default:
                Debug.Log("Invalid state" + curState);
                break;
        }
        Debug.Log("Curstate = " + curState);
    }
}
