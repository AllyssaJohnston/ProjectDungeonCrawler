using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

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
    [SerializeField] int enemyXOffset;
    [SerializeField] int enemyYOffset;
    [SerializeField] int enemyRotation;
    [SerializeField] int spellXOffset;
    [SerializeField] int spellYOffset;
    [SerializeField] int spellRotation;
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
    }

    private ArrowIndicatorManagerBehavior() {}

    public static void nextState(E_State nextState)
    {
        switch (nextState)
        {
            case E_State.PLAYER_SPELL_SELECTION:
                deleteArrows();
                //foreach (FriendlySpellBehavior spell in PartySpellManagerBehavior.spells)
                //{
                //    if (spell.canCast)
                //    {
                //        createArrow(E_Arrow_Type.SPELL_PTR, spell.gameObject);
                //    }
                //}
                break;
            case E_State.PLAYER_ENEMY_TARGET_SELECTION:
                deleteArrows();
                foreach (EnemyBehavior enemy in CombatManagerBehavior.enemyCharacterBehaviors)
                {
                    createArrow(E_Arrow_Type.ENEMY_PTR, instance.fullScreenPanel, enemy.gameObject.transform.position);
                }
                break;
            case E_State.PLAYER_BETWEEN_SPELLS_BUFFFER:
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
        //GameObject curArrow = Instantiate<GameObject>(instance.arrow);
        //Quaternion rot = Quaternion.identity;

        //Vector3 scale = curArrow.transform.localScale;
        //curArrow.transform.SetParent(parent.transform);
        //curArrow.transform.localScale = scale;
        //switch (type)
        //{
        //    case (E_Arrow_Type.ENEMY_PTR):
        //        curArrow.transform.localPosition -= new Vector3(instance.enemyXOffset, instance.enemyYOffset, 0);

        //        rot = Quaternion.Euler(0, 0, instance.enemyRotation);
        //        break;
        //    case (E_Arrow_Type.SPELL_PTR):
        //        curArrow.transform.localPosition -= new Vector3(instance.spellXOffset, instance.spellYOffset, 0);

        //        rot = Quaternion.Euler(0, 0, instance.enemyRotation);
        //        break;
        //    default:
        //        Debug.Log("unrecognized arrow type");
        //        break;
        //}
        //curArrow.transform.rotation = rot;
        //curArrow.SetActive(true);
        //arrows.Add(curArrow);
    }

    private static void createArrow(E_Arrow_Type type, GameObject parent, Vector3 refPos)
    {
        GameObject curArrow = Instantiate(instance.arrow);
        Vector3 scale = curArrow.transform.localScale;
        curArrow.transform.SetParent(parent.transform);
        curArrow.transform.localScale = scale;

        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.identity;
        switch (type)
        {
            case (E_Arrow_Type.ENEMY_PTR):
                pos = new Vector3(refPos.x + instance.enemyXOffset, instance.enemyYOffset, 0);
                rot = Quaternion.Euler(0, 0, instance.enemyRotation);
                break;
            case (E_Arrow_Type.SPELL_PTR):
                pos = new Vector3(refPos.x + instance.spellXOffset, instance.spellYOffset, 0);
                rot = Quaternion.Euler(0, 0, instance.enemyRotation);
                break;
            default:
                Debug.Log("unrecognized arrow type");
                break;
        }
        curArrow.transform.position = pos;
        curArrow.transform.rotation = rot;
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
