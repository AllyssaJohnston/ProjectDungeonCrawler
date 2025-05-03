using UnityEngine;

public enum E_SPELL_TARGETING
{
    HIGHEST_HEALTH,
    LOWEST_HEALTH,
    HIGHEST_MORALE,
    LOWEST_MORALE,
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
