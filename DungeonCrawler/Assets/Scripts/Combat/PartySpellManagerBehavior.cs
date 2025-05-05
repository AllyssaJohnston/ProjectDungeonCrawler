using UnityEngine;

public class PartySpellManagerBehavior : MonoBehaviour
{
    private static PartySpellManagerBehavior instance;
    public static FriendlySpellBehavior[] spells;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        Debug.Log("party spell manager initialized");
    }

    public void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        spells = GetComponentsInChildren<FriendlySpellBehavior>();
        UpdateSpellOrder();
    }

    public static void UpdateSpellOrder()
    {
        int numCastable = 0;
        int numUncastable = 0;
        foreach (FriendlySpellBehavior spell in spells)
        {
            spell.updateCanCast();
            if (spell.canCast)
            {
                spell.gameObject.transform.SetSiblingIndex(numCastable);
                numCastable++;
            }
            else
            {
                spell.gameObject.transform.SetSiblingIndex(numCastable + numUncastable + 1);
                numUncastable++;
            }
        }
    }

}
