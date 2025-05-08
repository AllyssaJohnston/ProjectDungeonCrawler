using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

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
    [SerializeField] GameObject arrowsPanel;
    [SerializeField] GameObject button;
    [SerializeField] float enemySelectionXOffset;
    [SerializeField] float enemySelectionYOffset;
    [SerializeField] float enemyTurnXOffset;
    [SerializeField] float enemyTurnYOffset;
    [SerializeField] float enemyRotation;
    [SerializeField] float spellXOffset;
    [SerializeField] float spellYOffset;
    [SerializeField] float spellRotation;
    [SerializeField] float buttonXOffset;
    [SerializeField] float buttonYOffset;
    [SerializeField] float buttonRotation;
    private static List<GameObject> arrows = new List<GameObject>();


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

    public static void OnNextState(E_State nextState)
    {
        switch (nextState)
        {
            case E_State.PLAYER_SPELL_SELECTION:
                deleteArrows();
                FriendlySpellBehavior[] spells = PartySpellManagerBehavior.getSpells();

                foreach (FriendlySpellBehavior spell in spells)
                {
                    if (spell.canCast)
                    {
                        createArrow(E_Arrow_Type.SPELL_PTR, spell.gameObject);
                    }
                }
                createArrow(E_Arrow_Type.END_TURN_PTR, instance.button.gameObject);
                break;
            case E_State.PLAYER_ENEMY_TARGET_SELECTION:
                deleteArrows();
                foreach (EnemyBehavior enemy in CombatManagerBehavior.enemyCharacterBehaviors)
                {
                    createArrow(E_Arrow_Type.ENEMY_SELECTION_PTR, enemy.gameObject.transform.parent.gameObject);
                }
                break;
            case E_State.PLAYER_BETWEEN_SPELLS_BUFFFER:
            case E_State.PLAYER_END_TURN_BUFFER:
                deleteArrows();
                break;
            case E_State.ENEMY_BUFFER:
                deleteArrows();
                createArrow(E_Arrow_Type.ENEMY_TURN_PTR, instance.arrowsPanel);
                break;
            case E_State.ENEMY_END_TURN_BUFFER:
                break;
            default:
                break;
        }
    }
    private static void createArrow(E_Arrow_Type type, GameObject parent)
    {
        GameObject curArrow = Instantiate(instance.arrow);
        Vector3 scale = curArrow.transform.localScale;
        curArrow.transform.SetParent(parent.transform);
        RectTransform arrowRect = curArrow.GetComponent<RectTransform>();
        arrowRect.localScale = scale;
        // TODO scaling bug

        float zRot = 0;
        bool move = true;
        switch (type)
        {
            case (E_Arrow_Type.SPELL_PTR):
                arrowRect.anchoredPosition = new Vector3(instance.spellXOffset, instance.spellYOffset, 0);
                arrowRect.Rotate(0, 0, instance.spellRotation);
                zRot = instance.spellRotation;
                break;
            case (E_Arrow_Type.END_TURN_PTR):
                curArrow.transform.localPosition = new Vector3(instance.buttonXOffset, instance.buttonYOffset, 0);
                arrowRect.Rotate(0, 0, instance.buttonRotation);
                zRot = instance.buttonRotation;
                break;
            case (E_Arrow_Type.ENEMY_SELECTION_PTR):
                arrowRect.anchoredPosition = new Vector3(instance.enemySelectionXOffset, instance.enemySelectionYOffset, 0);
                arrowRect.Rotate(0, 0, instance.enemyRotation);
                zRot = instance.enemyRotation;
                break;
            case (E_Arrow_Type.ENEMY_TURN_PTR):
                arrowRect.anchoredPosition = new Vector3(instance.enemyTurnXOffset, instance.enemyTurnYOffset, 0);
                arrowRect.Rotate(0, 0, instance.enemyRotation);
                zRot = instance.enemyRotation;
                move = false;
                break;
            default:
                Debug.Log("unrecognized arrow type");
                break;
        }
        curArrow.GetComponent<ArrowIndicatorBehavior>().setUp(zRot, move);
        arrows.Add(curArrow);
    }

    public static void deleteArrows()
    {
        for (int i = arrows.Count - 1; i > -1; i--)
        {
            Destroy(arrows[i]);
        }
        arrows.Clear();
    }

}
