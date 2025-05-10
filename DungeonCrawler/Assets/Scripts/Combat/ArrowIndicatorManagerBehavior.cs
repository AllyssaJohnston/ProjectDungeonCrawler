using UnityEngine;
using System.Collections.Generic;

public enum E_Arrow_Type 
{
    SPELL_PTR,
    END_TURN_PTR,
    ENEMY_SELECTION_PTR,
    ENEMY_TURN_PTR
}

public class ArrowIndicatorManagerBehavior : MonoBehaviour
{
    private static ArrowIndicatorManagerBehavior instance;
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject button;
    [SerializeField] float enemySelectionXOffset;
    [SerializeField] float enemySelectionYOffset;
    [SerializeField] float enemyTurnXOffset;
    [SerializeField] float enemyTurnYOffset;
    [SerializeField] float enemyRotation;
    [SerializeField] float enemyMoveDist;
    [SerializeField] float enemyScale;
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
        arrows.Add(E_Arrow_Type.ENEMY_SELECTION_PTR, new List<GameObject> { });
        foreach (EnemyBehavior enemy in CombatManagerBehavior.enemyCharacterBehaviors)
        {
            arrows[E_Arrow_Type.ENEMY_SELECTION_PTR].Add(createArrow(E_Arrow_Type.ENEMY_SELECTION_PTR, enemy.gameObject.transform.parent.gameObject));
            enemyTurnArrows.Add(enemy, createArrow(E_Arrow_Type.ENEMY_TURN_PTR, enemy.gameObject.transform.parent.gameObject));
        }
    }

    // update arrow visibility based on cur state
    public static void OnNextState(E_State nextState)
    {
        switch (nextState)
        {
            case E_State.PLAYER_SPELL_SELECTION:
                updateEnemyTurnArrowVisibility(null);
                updateArrowVisibility(new List<E_Arrow_Type> { E_Arrow_Type.SPELL_PTR, E_Arrow_Type.END_TURN_PTR });
                break;
            case E_State.PLAYER_ENEMY_TARGET_SELECTION:
                updateArrowVisibility(new List<E_Arrow_Type> { E_Arrow_Type.ENEMY_SELECTION_PTR});
                break;
            case E_State.PLAYER_BETWEEN_SPELLS_BUFFFER:
            case E_State.PLAYER_END_TURN_BUFFER:
                updateArrowVisibility(new List<E_Arrow_Type> { });
                break;
            case E_State.ENEMY_BUFFER:
                updateArrowVisibility(new List<E_Arrow_Type> { });
                updateEnemyTurnArrowVisibility(CombatManagerBehavior.enemyCharacterBehaviors[StateManagerBehavior.curEnemyIndex]);
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
            case (E_Arrow_Type.ENEMY_SELECTION_PTR):
                arrowRect.localScale *= instance.enemyScale;
                arrowRect.anchoredPosition = new Vector3(instance.enemySelectionXOffset, instance.enemySelectionYOffset, 0);
                arrowRect.Rotate(0, 0, instance.enemyRotation);
                zRot = instance.enemyRotation;
                moveDist = instance.enemyMoveDist;
                break;
            case (E_Arrow_Type.ENEMY_TURN_PTR):
                arrowRect.localScale *= instance.enemyScale;
                arrowRect.anchoredPosition = new Vector3(instance.enemyTurnXOffset, instance.enemyTurnYOffset, 0);
                arrowRect.Rotate(0, 0, instance.enemyRotation);
                zRot = instance.enemyRotation;
                moveDist = 0f;
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

    private static void updateEnemyTurnArrowVisibility(EnemyBehavior curEnemy)
    {
        foreach(var pair in enemyTurnArrows) 
        {
            if (curEnemy != null && pair.Key == curEnemy)
            {
                enemyTurnArrows[curEnemy].SetActive(true);
            }
            else
            {
                enemyTurnArrows[pair.Key].SetActive(false);
            }
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
