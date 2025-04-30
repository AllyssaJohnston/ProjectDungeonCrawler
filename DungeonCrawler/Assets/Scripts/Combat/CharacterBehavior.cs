using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterBehavior : MonoBehaviour
{
    public bool friendly = true;
    [SerializeField] public Sprite iconSprite;
    private Sprite characterSprite;
    public int maxHealth = 100;
    protected int health;
    protected int maxMorale = 10;
    protected int morale;
    protected bool available = true;

    private void Awake()
    {
        characterSprite = GetComponent<SpriteRenderer>().sprite;
        health = maxHealth;
        morale = maxMorale;
    }

    public int getHealth() { return health; }

    public int getMorale() { return morale; }

    // updates the character's health
    public void updateHealth(int healthChange)
    {
        if (healthChange < 0)
        {
            StartCoroutine(takeDamageEffect());
        }
        health += healthChange;
        if (health <= 0)
        {
            health = 0;
            available = false;
        }
    }

    public IEnumerator takeDamageEffect()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(.2f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    // called at start of battle
    // use this method to reset things between fights
    virtual public void startBattle()
    {
        // morale regen
        // recover whatever is greater: half of your missing morale rounded down OR 1
        int moraleDif = maxMorale - morale;
        morale += Mathf.Max(moraleDif / 2, 1);
        if (morale > maxMorale)
        {
            morale = maxMorale;
        }

        // health regen
        // start with a minimum of 1 health
        health = Mathf.Max(health, 1);
    }

    // called at start of turn
    // use this method to reset things between turns
    virtual public void startTurn()
    {
        // set availability
        available = isActive();
    }

    // returns whether character can cast spells
    public bool canCast() { return available; }

    // returns whether character is 'in' the fight
    public bool isActive() { return health > 0 && morale > 0; }

    // take effects of casting a spell
    // and then makes the character unable to cast more spells
    public void cast(int moraleChange)
    {
        morale += moraleChange;
        available = false;
    }
}
