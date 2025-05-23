using UnityEngine;

public class PartySpellManagerBehavior : MonoBehaviour
{
    private static PartySpellManagerBehavior instance;
    [SerializeField] GameObject viewport;
    private static FriendlySpellBehavior[] spells;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        Debug.Log("party spell manager initialized");
    }

    // Re-fetch the spells and enable/disable based on tutorial mode
    public static void updateSpells(bool tutorialMode)
    {
        spells = instance.viewport.GetComponentsInChildren<FriendlySpellBehavior>(true);
        foreach(FriendlySpellBehavior spellBehavior in spells)
        {
            spellBehavior.gameObject.SetActive(spellBehavior.inTutorial && tutorialMode || !spellBehavior.inTutorial && !tutorialMode);
        }
    }

    // Put castable spells up top and uncastable spells at bottom
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

    // Reset the spell's text with the given damage modifier
    public static void UpdateSpellTextStats(float damageModifier)
    {
        foreach (FriendlySpellBehavior spell in spells)
        {
            spell.applyDamageModifier(damageModifier);
        }
    }

    public static FriendlySpellBehavior[] getSpells() { return spells; }
}
