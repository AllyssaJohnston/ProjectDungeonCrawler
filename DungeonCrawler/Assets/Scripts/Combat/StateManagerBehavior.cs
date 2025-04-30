using UnityEngine;

public enum E_State
{
    PLAYER_SPELL_SELECTION,
    PLAYER_ENEMY_TARGET_SELECTION,
    ENEMY_BUFFER,
    ENEMY_ACTION
}

public class StateManagerBehavior : MonoBehaviour
{
    public static StateManagerBehavior instance;
    private static E_State curState = E_State.PLAYER_SPELL_SELECTION;

    private static float bufferTimer = 0f;
    private static float waitTime = 3f;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private StateManagerBehavior() { }

    // Update is called once per frame
    void Update()
    {
        stateTick();   
    }

    // complete state effects and update if needed
    private static void stateTick()
    {
        switch (curState)
        {
            case E_State.PLAYER_SPELL_SELECTION:
                // do nothing, player needs to do things to end this state
                break;
            case E_State.PLAYER_ENEMY_TARGET_SELECTION:
                // do nothing, player needs to do things to end this state
                break;
            case E_State.ENEMY_BUFFER:
                bufferTimer += Time.deltaTime;
                if (bufferTimer > waitTime)
                {
                    NextState();
                    bufferTimer = 0f;
                }
                break;
            case E_State.ENEMY_ACTION:
                foreach(EnemyBehavior character in GameManagerBehavior.enemyCharacterBehaviors)
                {
                    character.startTurn();
                }
                foreach (EnemyBehavior character in GameManagerBehavior.enemyCharacterBehaviors) //TODO this could be simplified if we don't need all enemies to reset before each enemy casts a spell
                {
                    character.chooseSpell(GameManagerBehavior.friendlyCharacterBehaviors);
                }
                NextState();
                break;
            default:
                Debug.Log("Invalid state" + curState);
                break;
        }
    }

    // called at start of combat
    public static void StartBattle()
    {
        foreach(CharacterBehavior character in GameManagerBehavior.friendlyCharacterBehaviors)
        {
            character.startBattle();
        }
        foreach (EnemyBehavior character in GameManagerBehavior.enemyCharacterBehaviors)
        {
            character.startBattle();
        }
    }

    // go to the given state
    public static void NextState(E_State nextState)
    {
        if (curState == E_State.ENEMY_ACTION && nextState == E_State.PLAYER_SPELL_SELECTION)
        {
            GameManagerBehavior.playerStartTurn();
        }
        curState = nextState;
    }

    // go to the next state
    public static void NextState()
    {
        switch (curState)
        {
            case E_State.PLAYER_SPELL_SELECTION:
                // go to enmey state
                curState = E_State.PLAYER_ENEMY_TARGET_SELECTION;
                DebugBehavior.updateLog("pick an enemy target");
                break;
            case E_State.PLAYER_ENEMY_TARGET_SELECTION:
                // go to enmey state
                curState = E_State.PLAYER_SPELL_SELECTION;
                break;
            case E_State.ENEMY_BUFFER:
                curState = E_State.ENEMY_ACTION;
                break;
            case E_State.ENEMY_ACTION:
                // go to player state 
                GameManagerBehavior.playerStartTurn();
                curState = E_State.PLAYER_SPELL_SELECTION;
                break;
            default:
                Debug.Log("Invalid state" + curState);
                break;
        }
    }

    public static E_State getState() { return curState; }
}
