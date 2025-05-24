using UnityEngine;
using UnityEngine.UI;

public class SkipBufferButtonBehavior : MonoBehaviour
{
    private static SkipBufferButtonBehavior instance;
    private static Button button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        button = GetComponent<Button>();
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public static void OnNextState(E_State nextState)
    {
        if (button != null)
        {
            switch (nextState)
            {
                case E_State.ENEMY_BUFFER:
                case E_State.BETWEEN_ENEMIES_BUFFER:
                case E_State.ENEMY_ACTION:
                    //show if not in tutorial
                    button.gameObject.SetActive(!CombatManagerBehavior.inTutorial);
                    break;
                default:
                    // hide
                    button.gameObject.SetActive(false);
                    break;
            }
        }
    }

    public static void OnClick()
    {
        StateManagerBehavior.InteruptState();
    }
}
