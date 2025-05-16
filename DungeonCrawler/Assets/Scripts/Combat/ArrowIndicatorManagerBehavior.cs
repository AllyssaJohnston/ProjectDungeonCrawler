using UnityEngine;
using System.Collections.Generic;

public enum E_Arrow_Type 
{
    SPELL_PTR,
    END_TURN_PTR,
    CHAR_PTR,
}

public class ArrowIndicatorManagerBehavior : MonoBehaviour
{
    private static ArrowIndicatorManagerBehavior instance;
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject button;
    [SerializeField] float charSelectionXOffset;
    [SerializeField] float charSelectionYOffset;
    //[SerializeField] float enemyTurnXOffset;
    //[SerializeField] float enemyTurnYOffset;
    [SerializeField] float charRotation;
    [SerializeField] float charMoveDist;
    [SerializeField] float charScale;
    [SerializeField] float spellXOffset;
    [SerializeField] float spellYOffset;
    [SerializeField] float spellRotation;
    [SerializeField] float spellMoveDist;
    [SerializeField] float spellScale;
    [SerializeField] float buttonXOffset;
    [SerializeField] float buttonYOffset;
    [SerializeField] float buttonRotation;
    private static Dictionary<E_Arrow_Type, List<GameObject>> arrows = new Dictionary<E_Arrow_Type, List<GameObject>>();
    private static Dictionary<EnemyBehavior, GameObject> enemyTurnArrows = new Dictionary<EnemyBehavior, GameObject>();
    private static Dictionary<FriendlyBehavior, GameObject> friendlyTurnArrows = new Dictionary<FriendlyBehavior, GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        Debug.Log("arrow indicator manager initialized");
    }

    private ArrowIndicatorManagerBehavior() {}

    // get called at the start of combat
    public static void createArrows()
    {
        deleteArrows();

        //spell ptrs
        FriendlySpellBehavior[] spells = PartySpellManagerBehavior.getSpells();
        arrows.Add(E_Arrow_Type.SPELL_PTR, new List<GameObject> { });
        foreach (FriendlySpellBehavior spell in spells)
        {
            arrows[E_Arrow_Type.SPELL_PTR].Add(createArrow(E_Arrow_Type.SPELL_PTR, spell.gameObject));
        }

        //end turn button
        arrows.Add(E_Arrow_Type.END_TURN_PTR, new List<GameObject> { createArrow(E_Arrow_Type.END_TURN_PTR, instance.button.gameObject)  });

        //enemy turn and target selection
        foreach (EnemyBehavior enemy in CombatManagerBehavior.enemyCharacterBehaviors)
        {
            enemyTurnArrows.Add(enemy, createArrow(E_Arrow_Type.CHAR_PTR, enemy.gameObject.transform.parent.gameObject));
        }

        //enemy turn and target selection
        foreach (FriendlyBehavior friendly in CombatManagerBehavior.friendlyCharacterBehaviors)
        {
            friendlyTurnArrows.Add(friendly, createArrow(E_Arrow_Type.CHAR_PTR, friendly.gameObject.transform.parent.gameObject));
        }
    }

    // update arrow visibility based on cur state
    public static void OnNextState(E_State nextState)
    {
        switch (nextState)
        {
            case E_State.PLAYER_SPELL_SELECTION:
                updateEnemySelectionArrowVisibility(false);
                updateFriendlySelectionArrowVisibility(false);
                updateArrowVisibility(new List<E_Arrow_Type> { E_Arrow_Type.SPELL_PTR, E_Arrow_Type.END_TURN_PTR });
                break;
            case E_State.PLAYER_ENEMY_TARGET_SELECTION:
                updateArrowVisibility();
                updateFriendlySelectionArrowVisibility(false);
                updateEnemySelectionArrowVisibility(true);
                break;
            case E_State.PLAYER_FRIENDLY_TARGET_SELECTION:
                updateArrowVisibility();
                updateEnemySelectionArrowVisibility(false);
                updateFriendlySelectionArrowVisibility(true);
                break;
            case E_State.PLAYER_BETWEEN_SPELLS_BUFFFER:
            case E_State.PLAYER_END_TURN_BUFFER:
                updateEnemySelectionArrowVisibility(false);
                updateFriendlySelectionArrowVisibility(false);
                updateArrowVisibility();
                break;
            case E_State.ENEMY_BUFFER:
                updateArrowVisibility();
                updateEnemySelectionArrowVisibility(CombatManagerBehavior.enemyCharacterBehaviors[StateManagerBehavior.curEnemyIndex]);
                break;
            case E_State.ENEMY_END_TURN_BUFFER:
                break;
            default:
                break;
        }
    }

    private static GameObject createArrow(E_Arrow_Type type, GameObject parent)
    {
        GameObject curArrow = Instantiate(instance.arrow);
        Vector3 scale = curArrow.transform.localScale;
        curArrow.transform.SetParent(parent.transform);
        RectTransform arrowRect = curArrow.GetComponent<RectTransform>();
        arrowRect.localScale = scale;

        float zRot = 0;
        float moveDist = 0f;
        switch (type)
        {
            case (E_Arrow_Type.SPELL_PTR):
                arrowRect.localScale *= instance.spellScale;
                arrowRect.anchoredPosition = new Vector3(instance.spellXOffset, instance.spellYOffset, 0);
                arrowRect.Rotate(0, 0, instance.spellRotation);
                zRot = instance.spellRotation;
                moveDist = instance.spellMoveDist;
                break;
            case (E_Arrow_Type.END_TURN_PTR):
                arrowRect.localScale *= instance.spellScale;
                arrowRect.anchoredPosition = new Vector3(instance.buttonXOffset, instance.buttonYOffset, 0);
                arrowRect.Rotate(0, 0, instance.buttonRotation);
                zRot = instance.buttonRotation;
                moveDist = instance.spellMoveDist;
                break;
            case (E_Arrow_Type.CHAR_PTR):
                arrowRect.localScale *= instance.charScale;
                arrowRect.anchoredPosition = new Vector3(instance.charSelectionXOffset, instance.charSelectionYOffset, 0);
                arrowRect.Rotate(0, 0, instance.charRotation);
                zRot = instance.charRotation;
                moveDist = instance.charMoveDist;
                break;
            default:
                Debug.Log("unrecognized arrow type");
                break;
        }
        curArrow.GetComponent<ArrowIndicatorBehavior>().setUp(zRot, moveDist);
        return curArrow;
    }

    private static void updateArrowVisibility(List<E_Arrow_Type> visibleArrowTypes)
    {
        foreach(var pair in arrows)
        {
            List<GameObject> curArrows = pair.Value;
            foreach(GameObject arrow in curArrows)
            {
                arrow.SetActive(visibleArrowTypes.Contains(pair.Key));
            }
        }
    }

    private static void updateArrowVisibility()
    {
        foreach (var pair in arrows)
        {
            List<GameObject> curArrows = pair.Value;
            foreach (GameObject arrow in curArrows)
            {
                arrow.SetActive(false);
            }
        }
    }

    private static void updateEnemySelectionArrowVisibility(EnemyBehavior curEnemy)
    {
        foreach(var pair in enemyTurnArrows) 
        {
            if (curEnemy != null && pair.Key == curEnemy)
            {
                enemyTurnArrows[curEnemy].GetComponent<ArrowIndicatorBehavior>().UpdateMove(false);
                enemyTurnArrows[curEnemy].SetActive(true);
            }
            else
            {
                enemyTurnArrows[pair.Key].SetActive(false);
            }
        }
    }

    private static void updateEnemySelectionArrowVisibility(bool show)
    {
        foreach (var pair in enemyTurnArrows)
        {
            if (pair.Key.isAlive() && show == true)
            {
                enemyTurnArrows[pair.Key].GetComponent<ArrowIndicatorBehavior>().UpdateMove(true);
                enemyTurnArrows[pair.Key].SetActive(true);
            }
            else
            {
                enemyTurnArrows[pair.Key].SetActive(false);
            }
        }
    }

    private static void updateFriendlySelectionArrowVisibility(bool show)
    {
        foreach (var pair in friendlyTurnArrows)
        {

            friendlyTurnArrows[pair.Key].GetComponent<ArrowIndicatorBehavior>().UpdateMove(show);
            friendlyTurnArrows[pair.Key].SetActive(show);
        }
    }

    private static void deleteArrows()
    {
        foreach (var pair in arrows)
        {
            List<GameObject> curArrows = pair.Value;
            for (int i = curArrows.Count - 1; i > -1; i--)
            {
                Destroy(curArrows[i]);
            }
        }
        arrows.Clear();

        foreach (var pair in enemyTurnArrows)
        {
            Destroy(pair.Value);
        }
        enemyTurnArrows.Clear();
    }
}
