using UnityEngine;

public class SpellBehavior : MonoBehaviour
{
    [Header("Spell details")]
    public string spellName = "unnamed";
    public int damage = 10;
    public int heal = 0;
    public bool damageAllEnemies = false;

    [HideInInspector] public string spellDescriptionText;
    [HideInInspector] public string castingCharactersText;

    private void Start()
    {
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
