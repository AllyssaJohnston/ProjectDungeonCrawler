using UnityEngine;

public enum E_State
{
    PLAYER_SPELL_SELECTION,
    PLAYER_ENEMY_TARGET_SELECTION,
    PLAYER_BETWEEN_SPELLS_BUFFFER,
    PLAYER_END_TURN_BUFFER,
    ENEMY_BUFFER,
    BETWEEN_ENEMIES_BUFFER,
    ENEMY_ACTION,
    ENEMY_END_TURN_BUFFER,
    COMBAT_ENDED
}

public class StateManagerBehavior : MonoBehaviour
{
    private static StateManagerBehavior instance;

    private static E_State curState = E_State.PLAYER_SPELL_SELECTION;

    public static int curEnemyIndex = 0;
    private static float bufferTimer = 0f;
    [SerializeField] float playerBetweenSpellsWaitTime = 2f;
    [SerializeField] float enemyActionWaitTime = 1.5f;
    [SerializeField] float enemyBetweenTurnsWaitTime = 2f;
    [SerializeField] float playerEndTurnBuffer = .75f;

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
            case E_State.PLAYER_ENEMY_TARGET_SELECTION:
                // do nothing, player needs to do things to end this state
                break;
            case E_State.PLAYER_BETWEEN_SPELLS_BUFFFER:
                buffer(instance.playerBetweenSpellsWaitTime);
                break;
            case E_State.PLAYER_END_TURN_BUFFER:
                // prep/reset enemies for next round
                curEnemyIndex = 0;
                CombatManagerBehavior.enemiesStartTurn();
                buffer(instance.playerEndTurnBuffer);
                break;
            case E_State.ENEMY_BUFFER:
                buffer(instance.enemyActionWaitTime);
                break;
            case E_State.BETWEEN_ENEMIES_BUFFER:
                buffer(instance.enemyBetweenTurnsWaitTime);
                break;
            case E_State.ENEMY_ACTION:
                CombatManagerBehavior.enemyCharacterBehaviors[curEnemyIndex].castSpell(CombatManagerBehavior.friendlyCharacterBehaviors);
                if (curEnemyIndex < CombatManagerBehavior.enemyCharacterBehaviors.Count - 1)
                {
                    // continue rotating through enemies
                    curEnemyIndex++;
                    NextState(E_State.BETWEEN_ENEMIES_BUFFER);
                    if (CombatManagerBehavior.enemyCharacterBehaviors[curEnemyIndex].canCast() == false)
                    {
                        InteruptState();
                    }
                    return;
                }
                NextState();
                break;
            case E_State.ENEMY_END_TURN_BUFFER:
                buffer(instance.enemyBetweenTurnsWaitTime);
                break;
            case E_State.COMBAT_ENDED:
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
            bufferTimer = 0f;
            NextState(); 
        }
    }

    // called at start of combat
    public static void StartBattle()
    {
        curEnemyIndex = 0;
        bufferTimer = 0f;
        NextState(E_State.PLAYER_SPELL_SELECTION);
    }

    // go to the given state
    public static void NextState(E_State nextState)
    {
        Debug.Log(nextState);
        curState = nextState;

        DebugBehavior.nextState(nextState);
        CombatManagerBehavior.nextState(nextState);
        ArrowIndicatorManagerBehavior.nextState(nextState);
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
            case E_State.PLAYER_BETWEEN_SPELLS_BUFFFER:
            case E_State.ENEMY_END_TURN_BUFFER:
                NextState(E_State.PLAYER_SPELL_SELECTION);
                break;
      
            case E_State.PLAYER_END_TURN_BUFFER:
            case E_State.BETWEEN_ENEMIES_BUFFER:
                NextState(E_State.ENEMY_BUFFER);
                break;

            case E_State.ENEMY_BUFFER:
                NextState(E_State.ENEMY_ACTION);
                break;

            case E_State.ENEMY_ACTION:
                NextState(E_State.ENEMY_END_TURN_BUFFER);
                break;
           
            default:
                Debug.Log("Invalid state" + curState);
                break;
        }
    }

    // go to the next state, resetting any internal timers
    public static void InteruptState()
    {
        bufferTimer = 0f;
        NextState();
    }

    public static E_State getState() { return curState; }
}
