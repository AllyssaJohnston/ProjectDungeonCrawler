using UnityEngine;

public class SpellBehavior : MonoBehaviour
{
    public string spellName = "unnamed";
    public int damage = 10;
    public int moraleDamage = 0; //to the casting character
    public int manaCost;
    public bool damageAllEnemies = false;
}
