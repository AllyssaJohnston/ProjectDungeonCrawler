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

}
