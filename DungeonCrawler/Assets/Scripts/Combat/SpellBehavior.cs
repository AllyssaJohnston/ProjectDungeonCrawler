using UnityEngine;

public class SpellBehavior : MonoBehaviour
{
    [Header("Spell details")]
    public string spellName = "unnamed";
    public int damage = 10;
    public int moraleDamageToSelf = 0; // to the casting character
    public int heal = 0;
    public int manaCost = 0;
    public bool damageAllEnemies = false;
    //public bool targetEnemies = true; // false means target friendlies
}
