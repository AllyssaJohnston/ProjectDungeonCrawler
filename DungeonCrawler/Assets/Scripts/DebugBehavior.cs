using TMPro;
using UnityEngine;

public class DebugBehavior : MonoBehaviour
{
    private GameManagerBehavior gameManager;
    private StateManagerBehavior stateManager;
    public TMP_Text stateText;

    private void Start()
    {
        stateManager = FindFirstObjectByType<StateManagerBehavior>();
        gameManager = FindFirstObjectByType<GameManagerBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        stateText.text = "Cur state: " + stateManager.curState;
    }
}
