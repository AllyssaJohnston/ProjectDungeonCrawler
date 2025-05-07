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
    public bool damageAllEnemies = false;
    public E_SPELL_TARGETING targeting;
}

public class EnemySpellBehavior : SpellBehavior
{
    public E_SPELL_TARGETING targeting;

    public void setUpFromStat(EnemySpellStats curStat)
    {     
        spellName = curStat.spellName;
        damage = curStat.damage;
        moraleDamage = 0;
        manaCost = 0;
        damageAllEnemies = curStat.damageAllEnemies;
        targeting = curStat.targeting;
    }

}
