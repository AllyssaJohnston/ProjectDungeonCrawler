using UnityEngine;

public class TutorialPanelBehavior : MonoBehaviour
{
    public E_Tutorial_Action action;
    public E_Tutorial_Action additionalAction;
    public float timeDelay;
    public bool waitForState;
    public E_State startState;

    private float bufferTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void buffer()
    {
        if (waitForState && StateManagerBehavior.getState() != startState)
        {
            return;
        }
        bufferTimer += Time.deltaTime;
        if (bufferTimer > timeDelay)
        {
            gameObject.SetActive(true);
            switch(additionalAction)
            {
                case E_Tutorial_Action.SHOW_HELP:
                    HelpManagerBehavior.showButton(true);
                    break;
                default:
                    break;
            }
        }
    }
}
