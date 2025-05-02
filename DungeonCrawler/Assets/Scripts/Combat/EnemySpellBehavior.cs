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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
