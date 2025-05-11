using UnityEngine;

public enum E_SPELL_TARGETING
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
    public int moraleDamageToEnemies = 0; // to the opposing side
    public bool damageAllEnemies = false;
    public E_SPELL_TARGETING targeting;
}

public class EnemySpellBehavior : SpellBehavior
{
    public E_SPELL_TARGETING targeting;
    public int moraleDamageToEnemies = 0; // to the opposing side

    public void setUpFromStat(EnemySpellStats curStat)
    {     
        spellName = curStat.spellName;
        damage = curStat.damage;
        moraleDamageToSelf = 0;
        moraleDamageToEnemies = curStat.moraleDamageToEnemies; // to the opposing side
        manaCost = 0;
        damageAllEnemies = curStat.damageAllEnemies;
        targeting = curStat.targeting;
    }

}
