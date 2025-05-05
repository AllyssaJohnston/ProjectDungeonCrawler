using UnityEngine;
using System.Collections.Generic;

public enum E_Arrow_Type 
{
    SPELL_PTR,
    ENEMY_PTR,
}

public class ArrowIndicatorManagerBehavior : MonoBehaviour
{
    private static ArrowIndicatorManagerBehavior instance;
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject fullScreenPanel;
    [SerializeField] float enemyXOffset;
    [SerializeField] float enemyYOffset;
    [SerializeField] float enemyRotation;
    [SerializeField] float spellXOffset;
    [SerializeField] float spellYOffset;
    [SerializeField] float spellRotation;
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

    private void Update()
    {
        
    }

    public static void nextState(E_State nextState)
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
                break;
            case E_State.PLAYER_ENEMY_TARGET_SELECTION:
                deleteArrows();
                foreach (EnemyBehavior enemy in CombatManagerBehavior.enemyCharacterBehaviors)
                {
                    createArrow(E_Arrow_Type.ENEMY_PTR, instance.fullScreenPanel, enemy.gameObject.transform.position);
                }
                break;
            case E_State.PLAYER_BETWEEN_SPELLS_BUFFFER:
            case E_State.PLAYER_END_TURN_BUFFER:
                deleteArrows();
                break;
            case E_State.ENEMY_BUFFER:
                deleteArrows();
                createArrow(E_Arrow_Type.ENEMY_PTR, instance.fullScreenPanel, CombatManagerBehavior.enemyCharacterBehaviors[StateManagerBehavior.curEnemyIndex].gameObject.transform.position);
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
        curArrow.transform.localScale = scale;

        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.identity;
        float zRot = 0;
        switch (type)
        {
            case (E_Arrow_Type.SPELL_PTR):
                pos = new Vector3(instance.spellXOffset, instance.spellYOffset, 0);
                rot = Quaternion.Euler(0, 0, instance.spellRotation);
                zRot = instance.spellRotation;
                break;
            default:
                Debug.Log("unrecognized arrow type");
                break;
        }
        curArrow.transform.localPosition = pos;
        curArrow.transform.rotation = rot;
        curArrow.GetComponent<ArrowIndicatorBehavior>().setUp(zRot);
        arrows.Add(curArrow);
    }

    private static void createArrow(E_Arrow_Type type, GameObject parent, Vector3 refPos)
    {
        GameObject curArrow = Instantiate(instance.arrow);
        Vector3 scale = curArrow.transform.localScale;
        curArrow.transform.SetParent(parent.transform);
        curArrow.transform.localScale = scale;

        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.identity;
        float zRot = 0;
        switch (type)
        {
            case (E_Arrow_Type.ENEMY_PTR):
                pos = new Vector3(refPos.x + instance.enemyXOffset, instance.enemyYOffset, 0);
                rot = Quaternion.Euler(0, 0, instance.enemyRotation);
                zRot = instance.enemyRotation;
                break;
            default:
                Debug.Log("unrecognized arrow type");
                break;
        }
        curArrow.transform.position = pos;
        curArrow.transform.rotation = rot;
        curArrow.GetComponent<ArrowIndicatorBehavior>().setUp(zRot);
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
