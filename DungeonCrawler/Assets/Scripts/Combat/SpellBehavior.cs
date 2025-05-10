using UnityEngine;

public class SpellBehavior : MonoBehaviour
{
    public string spellName = "unnamed";
    public int damage = 10;
    public int moraleDamageToEnemies = 0; // to the opposing side
    public int moraleDamageToSelf = 0; // to the casting character
    public int manaCost = 0;
    public bool damageAllEnemies = false;
}
