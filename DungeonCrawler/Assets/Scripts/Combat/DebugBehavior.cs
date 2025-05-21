using TMPro;
using UnityEngine;

// Singleton
public class DebugBehavior : MonoBehaviour
{
    private static DebugBehavior instance;

    private bool lastFrameDebugMode = true;
    public bool debugMode = false;
    [SerializeField] TMP_Text stateText;
    [SerializeField] TMP_Text debugLog;

    private static string curLog;
    [SerializeField] int statementPosY = 0; // pos of the cur statement outside of debug mode
    [SerializeField] int statementDebugPosY = -35; // pos of the cur statement in debug mode

    private RectTransform statementRect;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        Debug.Log("debug info manager initialized");

        statementRect = debugLog.GetComponent<RectTransform>();
    }

    private DebugBehavior() {}

    // Update is called once per frame
    void Update()
    {
        if (lastFrameDebugMode != debugMode)
        {
            if (debugMode)
            {
                stateText.color = Color.black;
                stateText.text = StateManagerBehavior.getState().ToString();
                statementRect.anchoredPosition = new Vector3(statementRect.anchoredPosition.x, statementDebugPosY, 0);
            }
            else
            {
                stateText.color = new Color(0, 0, 0, 0);
                statementRect.anchoredPosition = new Vector3(statementRect.anchoredPosition.x, statementPosY, 0);
            }
        }
        lastFrameDebugMode = debugMode;
    }

    public static void OnNextState(E_State nextState)
    {
        switch (nextState)
        {
            case E_State.PLAYER_SPELL_SELECTION:
                updateLog(CombatManagerBehavior.inTutorial ? "" : "Choose a spell to cast or select end turn.");
                break;
            case E_State.PLAYER_ENEMY_TARGET_SELECTION:
                updateLog(CombatManagerBehavior.inTutorial ? "" : "Pick who to attack.");
                break;
            case E_State.PLAYER_END_TURN_BUFFER:
                updateLog("");
                break;
            case E_State.ENEMY_BUFFER:
                updateLog(CombatManagerBehavior.enemyCharacterBehaviors[StateManagerBehavior.curEnemyIndex].characterName + " is choosing their attack...");
                break;
            case E_State.ENEMY_END_TURN_BUFFER:
                break;
            default:
                break;

        }
    }

    public static void updateLog(string log) 
    {
        Debug.Log(log);
        curLog = log;
        if (instance != null)
        {
            instance.debugLog.text = curLog;
        }
    }
}
