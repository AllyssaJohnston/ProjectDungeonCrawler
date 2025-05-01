using TMPro;
using UnityEngine;

// Singleton
public class DebugBehavior : MonoBehaviour
{
    private static DebugBehavior instance;

    public TMP_Text stateText;
    public TMP_Text debugLog;

    private static string curLog;

    private void Awake()
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
        stateText.text = StateManagerBehavior.getState().ToString();
        debugLog.text = curLog;
    }

    public static void updateLog(string log) 
    {
        Debug.Log(log);
        curLog = log; 
    }
}
