using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyStats
{
    EnemyStats() { }

    public string characterName = "unnamed";
    public Sprite iconSprite;
    public Sprite characterSprite;

    public int maxHealth = 100;

    public List<EnemySpellStats> spells = new List<EnemySpellStats>();
}

[System.Serializable]
public class EnemySpellStats
{
    public string spellName = "unnamed";
    public int damage = 10;
    public bool damageAllEnemies = false;
    public E_SPELL_TARGETING targeting;
}

public class CombatEncounterBehavior : MonoBehaviour
{
    public List<EnemyStats> enemies = new List<EnemyStats>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
