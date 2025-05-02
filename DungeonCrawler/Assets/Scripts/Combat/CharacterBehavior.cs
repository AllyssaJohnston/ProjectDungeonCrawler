using System.Collections;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour
{
    public bool friendly = true;
    public string characterName = "unnamed";
    public Sprite iconSprite;
    private Sprite characterSprite;
    private SpriteRenderer characterSpriteRenderer;
    private Color regColor;
    public int maxHealth = 100;
    protected int health;
    protected int maxMorale = 10;
    protected int morale;
    protected bool available = true;

    private void Awake()
    {
        characterSpriteRenderer = GetComponent<SpriteRenderer>();
        characterSprite = characterSpriteRenderer.sprite;
        health = maxHealth;
        morale = maxMorale;
        regColor = characterSpriteRenderer.color;
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
        characterSpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.2f);
        characterSpriteRenderer.color = regColor;
    }

    // called at start of battle
    // use this method to reset things between fights
    virtual public void startBattle()
    {
        // morale regen
        // recover whatever is greater: half of your missing morale rounded down OR 1
        int moraleDif = maxMorale - morale;
        morale += Mathf.Max(moraleDif / 2, 1);
        morale = Mathf.Min(morale, maxMorale);

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
