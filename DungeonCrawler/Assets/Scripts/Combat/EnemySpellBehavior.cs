using UnityEngine;

public enum E_ENEMY_SPELL_TARGETING
{
    HIGHEST_HEALTH,
    LOWEST_HEALTH,
    HIGHEST_MORALE,
    LOWEST_MORALE,
}

[System.Serializable]
public class EnemySpellStats
{
    public string spellName = "unnamed";
    public int damage = 10;
    public int heal = 0;
    public int moraleDamageToEnemies = 0; // to the opposing side
    public bool damageAllEnemies = false;
    public E_ENEMY_SPELL_TARGETING characterTargeting;

    public string spellDescriptionText;

    public void setUpStringStats()
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

        if (moraleDamageToEnemies != 0)
        {
            spellDescriptionText += " for " + moraleDamageToEnemies + " morale damage,";
        }
    }

}
