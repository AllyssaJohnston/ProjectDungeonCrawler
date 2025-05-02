using UnityEngine;

public enum E_State
{
    PLAYER_SPELL_SELECTION,
    PLAYER_ENEMY_TARGET_SELECTION,
    PLAYER_END_TURN_BUFFER,
    ENEMY_BUFFER,
    ENEMY_ACTION,
    ENEMY_END_TURN_BUFFER
}

public class StateManagerBehavior : MonoBehaviour
{
    private static StateManagerBehavior instance;

    private static E_State curState = E_State.PLAYER_SPELL_SELECTION;

    private static int curEnemyIndex = 0;
    private static float bufferTimer = 0f;
    public static float enemyActionWaitTime = 2f;
    public static float playerToEnemyStateChangeWaitTime = .5f;
    public static float enemyToPlayerStateChangeWaitTime = 1f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        Debug.Log("state manager initialized");
    }

    public void Start()
    {
        if (instance != null && instance != this)
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
            case E_State.PLAYER_END_TURN_BUFFER:
                // prep/reset enemies for next round
                curEnemyIndex = 0;
                CombatManagerBehavior.enemiesStartTurn();
                buffer(playerToEnemyStateChangeWaitTime);
                break;
            case E_State.ENEMY_BUFFER:
                buffer(enemyActionWaitTime);
                break;
            case E_State.ENEMY_ACTION:
                CombatManagerBehavior.enemyCharacterBehaviors[curEnemyIndex].chooseSpell(CombatManagerBehavior.friendlyCharacterBehaviors);
                if (curEnemyIndex < CombatManagerBehavior.enemyCharacterBehaviors.Count - 1)
                {
                    // continue rotating through enemies
                    curEnemyIndex++;
                    NextState(E_State.ENEMY_BUFFER);
                    return;
                }
                NextState();
                break;
            case E_State.ENEMY_END_TURN_BUFFER:
                buffer(enemyToPlayerStateChangeWaitTime);
                break;
            default:
                Debug.Log("Invalid state" + curState);
                break;
        }
    }

    // update the buffer timer and go to the next state once past goalWaitTime
    private static void buffer(float goalWaitTime)
    {
        bufferTimer += Time.deltaTime;
        if (bufferTimer > goalWaitTime)
        {
            NextState();
            bufferTimer = 0f;
        }
    }

    // called at start of combat
    public static void StartBattle()
    {
        curEnemyIndex = 0;
        bufferTimer = 0f;
        foreach (CharacterBehavior character in CombatManagerBehavior.friendlyCharacterBehaviors)
        {
            character.startBattle();
        }
        foreach (EnemyBehavior character in CombatManagerBehavior.enemyCharacterBehaviors)
        {
            character.startBattle();
        }
        NextState(E_State.PLAYER_SPELL_SELECTION);
    }

    // go to the given state
    public static void NextState(E_State nextState)
    {
        switch(nextState)
        {
            case E_State.PLAYER_SPELL_SELECTION:
                EnemyTurnIndicatorBehavior.show(false);
                DebugBehavior.updateLog("Choose a spell to cast or select end turn.");
                break;
            case E_State.PLAYER_ENEMY_TARGET_SELECTION:
                EnemyTurnIndicatorBehavior.show(false);
                DebugBehavior.updateLog("Pick who to attack.");
                break;
            case E_State.ENEMY_BUFFER:
                EnemyTurnIndicatorBehavior.show(true);
                EnemyTurnIndicatorBehavior.Move(CombatManagerBehavior.enemyCharacters[curEnemyIndex].transform.position.x);
                DebugBehavior.updateLog(CombatManagerBehavior.enemyCharacterBehaviors[curEnemyIndex].characterName + " is choosing their attack...");
                break;
            case E_State.ENEMY_END_TURN_BUFFER:
                CombatManagerBehavior.playerStartTurn();
                break;
            default:
                break;

        }
        curState = nextState;
    }

    // go to the next state
    public static void NextState()
    {
        switch (curState)
        {
            case E_State.PLAYER_SPELL_SELECTION:
                NextState(E_State.PLAYER_ENEMY_TARGET_SELECTION);
                break;
            case E_State.PLAYER_ENEMY_TARGET_SELECTION:
                NextState(E_State.PLAYER_SPELL_SELECTION);
                break;
            case E_State.PLAYER_END_TURN_BUFFER:
                NextState(E_State.ENEMY_BUFFER);
                break;
            case E_State.ENEMY_BUFFER:
                NextState(E_State.ENEMY_ACTION);
                break;
            case E_State.ENEMY_ACTION:
                NextState(E_State.ENEMY_END_TURN_BUFFER);
                break;
            case E_State.ENEMY_END_TURN_BUFFER:
                NextState(E_State.PLAYER_SPELL_SELECTION);
                break;
            default:
                Debug.Log("Invalid state" + curState);
                break;
        }
    }

    public static E_State getState() { return curState; }
}
