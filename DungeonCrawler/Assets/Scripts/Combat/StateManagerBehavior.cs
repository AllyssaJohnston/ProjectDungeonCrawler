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
    private static StateManagerBehavior instance;

    private static E_State curState = E_State.PLAYER_SPELL_SELECTION;

    private static int curEnemyIndex = 0;
    private static float bufferTimer = 0f;
    private static float waitTime = 2f;

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
            case E_State.ENEMY_BUFFER:

                bufferTimer += Time.deltaTime;
                if (bufferTimer > waitTime)
                {
                    NextState();
                    bufferTimer = 0f;
                }
                break;
            case E_State.ENEMY_ACTION:
                CombatManagerBehavior.enemyCharacterBehaviors[curEnemyIndex].chooseSpell(CombatManagerBehavior.friendlyCharacterBehaviors);
                if (curEnemyIndex == CombatManagerBehavior.enemyCharacterBehaviors.Count - 1)
                {
                    // all enemies have gone, go back to player states
                    curEnemyIndex = 0;
                    // reset enemies for next round
                    foreach (EnemyBehavior character in CombatManagerBehavior.enemyCharacterBehaviors)
                    {
                        character.startTurn();
                    }
                    NextState();
                }
                else
                {
                    // continue rotating through enemies
                    curEnemyIndex++;
                    NextState(E_State.ENEMY_BUFFER);
                }
                break;
            default:
                Debug.Log("Invalid state" + curState);
                break;
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
    }

    // go to the given state
    public static void NextState(E_State nextState)
    {
        if (nextState == E_State.PLAYER_SPELL_SELECTION || nextState == E_State.PLAYER_ENEMY_TARGET_SELECTION)
        {
            EnemyTurnIndicatorBehavior.show(false);
        }
        if (nextState == E_State.PLAYER_ENEMY_TARGET_SELECTION)
        {
            DebugBehavior.updateLog("pick an enemy target");
        }
        if (curState == E_State.ENEMY_ACTION && nextState == E_State.PLAYER_SPELL_SELECTION)
        {
            CombatManagerBehavior.playerStartTurn();
        }
        if (nextState == E_State.ENEMY_BUFFER)
        {
            EnemyTurnIndicatorBehavior.show(true);
            EnemyTurnIndicatorBehavior.Move(CombatManagerBehavior.enemyCharacters[curEnemyIndex].transform.position.x);
            DebugBehavior.updateLog("enemy choosing spell, wait " + waitTime);
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
            case E_State.ENEMY_BUFFER:
                NextState(E_State.ENEMY_ACTION);
                break;
            case E_State.ENEMY_ACTION:
                NextState(E_State.PLAYER_SPELL_SELECTION);
                break;
            default:
                Debug.Log("Invalid state" + curState);
                break;
        }
    }

    public static E_State getState() { return curState; }
}
