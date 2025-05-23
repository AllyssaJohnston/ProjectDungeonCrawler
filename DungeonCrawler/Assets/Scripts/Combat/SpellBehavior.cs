using UnityEngine;

public enum E_SPELL_TARGETING
{
    SINGLE_ENEMY,
    ALL_ENEMIES,
    SINGLE_FRIENDLY,
    ALL_FRIENDLIES,
}

public class SpellBehavior : MonoBehaviour
{
    [Header("Spell details")]
    public string spellName = "unnamed";
    [HideInInspector] public float baseDamage;
    public float damage = 10;
    public int heal = 0;
    public E_SPELL_TARGETING targeting = E_SPELL_TARGETING.SINGLE_ENEMY;

    [HideInInspector] public string spellDescriptionText;
    [HideInInspector] public string castingCharactersText;

    private void Start()
    {
        baseDamage = damage;
        setUpStringStats();
    }

    virtual protected void setUpStringStats()
    {
        spellDescriptionText = spellName;

        if (damage != 0f)
        {
            spellDescriptionText += " for " + damage + " damage,";
        }

        if (heal != 0f)
        {
            spellDescriptionText += " for " + heal + " party health regen,";
        }
    }
}
