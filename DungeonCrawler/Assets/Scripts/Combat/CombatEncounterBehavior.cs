using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyStats
{
    EnemyStats() { }

    public string characterName = "unnamed";
    public Sprite characterSprite;
    public Sprite characterDamagedSprite;
    public Sprite characterUsedSprite;
    public Sprite characterDeadSprite;
    public float imageWidth;
    public float imageHeight;

    public int maxHealth = 100;

    public List<EnemySpellStats> spells = new List<EnemySpellStats>();
}

public class CombatEncounterBehavior : MonoBehaviour
{
    public List<EnemyStats> enemies = new List<EnemyStats>();
}
