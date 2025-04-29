using TMPro;
using UnityEngine;

public class DebugBehavior : MonoBehaviour
{
    public static DebugBehavior instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private DebugBehavior() {}

    private GameManagerBehavior gameManager;
    private StateManagerBehavior stateManager;
    public TMP_Text stateText;
    public TMP_Text debugLog;
    private static string curLog;

    private void Start()
    {
        stateManager = FindFirstObjectByType<StateManagerBehavior>();
        gameManager = FindFirstObjectByType<GameManagerBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        stateText.text = "Cur state: " + stateManager.curState;
        debugLog.text = curLog;
    }

    public static void updateLog(string log)
    {
        curLog = log;
    }
}
