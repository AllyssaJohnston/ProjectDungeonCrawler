using UnityEngine;

public enum E_Tutorial_Action
{
    CLICK_TO_CONTINUE = 0,
    CLICK_BLAST_CURSES = 1,
    CLICK_ENERGIZE = 2,
    CLICK_DEATHS_KISS = 3,
    CLICK_END_TURN = 4,
    CLICK_ENEMY_TARGET = 5,
}

public class TutorialManagerBehavior : MonoBehaviour
{
    private static TutorialManagerBehavior instance;

    private static TutorialPanelBehavior[] tutorialPanels;
    private static int curPanel = 0;

    public GameObject blastCursesSpell;
    public GameObject energizeSpell;
    public GameObject deathsKissSpell;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        tutorialPanels = instance.GetComponentsInChildren<TutorialPanelBehavior>(true);
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

    public static void startTutorial()
    {
        curPanel = 0;
        EndTurnButtonBehavior.OnNextState(StateManagerBehavior.getState());
        SkipBufferButtonBehavior.OnNextState(StateManagerBehavior.getState());
    }

    private void Update()
    {
        if (CombatManagerBehavior.inTutorial)
        {
            tutorialPanels[curPanel].buffer();
        }
    }


    public static void panelClicked()
    {
        if (tutorialPanels[curPanel].action == E_Tutorial_Action.CLICK_TO_CONTINUE)
        {
            nextPanel();
        }
    }

    private static void nextPanel()
    {
        tutorialPanels[curPanel].gameObject.SetActive(false);
        if (curPanel == tutorialPanels.Length - 1)
        {
            CombatManagerBehavior.inTutorial = false;
            ArrowIndicatorManagerBehavior.createArrows(CombatManagerBehavior.inTutorial);
            ArrowIndicatorManagerBehavior.OnNextState(StateManagerBehavior.getState());
            DebugBehavior.OnNextState(StateManagerBehavior.getState());
            EndTurnButtonBehavior.OnNextState(StateManagerBehavior.getState());
            SkipBufferButtonBehavior.OnNextState(StateManagerBehavior.getState());
        }
        if (curPanel < tutorialPanels.Length - 1)
        {
            curPanel++;
            EndTurnButtonBehavior.OnNextState(StateManagerBehavior.getState());
            SkipBufferButtonBehavior.OnNextState(StateManagerBehavior.getState());
        }
    }

    public static bool isValidSpell(FriendlySpellBehavior spell)
    {
        bool isValid = false;
        switch(tutorialPanels[curPanel].action)
        {
            case E_Tutorial_Action.CLICK_TO_CONTINUE:
                break;
            case E_Tutorial_Action.CLICK_BLAST_CURSES:
                isValid =  spell.gameObject == instance.blastCursesSpell;
                break;
            case E_Tutorial_Action.CLICK_ENERGIZE:
                isValid = spell.gameObject == instance.energizeSpell;
                break;
            case E_Tutorial_Action.CLICK_DEATHS_KISS:
                isValid = spell.gameObject == instance.deathsKissSpell;
                break;
            default:
                break;
        }
        if (isValid)
        {
            nextPanel();
        }
        return isValid;
    }

    public static bool canSelectEnemy()
    {
        if (tutorialPanels[curPanel].action == E_Tutorial_Action.CLICK_ENEMY_TARGET)
        {
            nextPanel();
            return true;
        }
        return false;
    }

    public static bool showButton()
    {
        return tutorialPanels[curPanel].action == E_Tutorial_Action.CLICK_END_TURN;
    }

    public static bool canEndTurn()
    {
        if (tutorialPanels[curPanel].action == E_Tutorial_Action.CLICK_END_TURN)
        {
            nextPanel();
            return true;
        }
        return false;
    }
}
