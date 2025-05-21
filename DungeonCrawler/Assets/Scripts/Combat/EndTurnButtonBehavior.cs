using UnityEngine;
using UnityEngine.UI;

public class EndTurnButtonBehavior : MonoBehaviour
{
    private static EndTurnButtonBehavior instance;
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
                case E_State.PLAYER_SPELL_SELECTION:
                    // show if not in tutorial
                    // show if in tutorial
                    button.gameObject.SetActive(!CombatManagerBehavior.inTutorial || (CombatManagerBehavior.inTutorial && TutorialManagerBehavior.showButton()));
                    break;
                case E_State.PLAYER_BETWEEN_SPELLS_BUFFFER:
                case E_State.ENEMY_END_TURN_BUFFER:
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
}
