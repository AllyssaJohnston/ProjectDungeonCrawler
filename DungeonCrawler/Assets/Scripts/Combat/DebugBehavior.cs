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
    [SerializeField] int infoPosY = -10; // pos of the cur statement outside of mode
    private int debugInfoPosY = -35; // pos of the cur statement in debug mode

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
    }

    private DebugBehavior() {}

    // Update is called once per frame
    void Update()
    {
        if (lastFrameDebugMode != debugMode)
        {
            Vector3 pos = debugLog.transform.localPosition;
            if (debugMode)
            {
                stateText.color = Color.black;
                stateText.text = StateManagerBehavior.getState().ToString();
                debugLog.transform.localPosition = new Vector3(pos.x, debugInfoPosY, 0);
            }
            else
            {
                stateText.color = new Color(0, 0, 0, 0);
                debugLog.transform.localPosition = new Vector3(pos.x, infoPosY, 0);
            }
        }
        lastFrameDebugMode = debugMode;
    }

    public static void updateLog(string log) 
    {
        Debug.Log(log);
        curLog = log;
        instance.debugLog.text = curLog;
    }
}
