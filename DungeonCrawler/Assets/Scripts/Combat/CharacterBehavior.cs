using System.Collections;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour
{
    protected bool friendly = true;
    public string characterName = "unnamed";
    public Sprite iconSprite;

    protected SpriteRenderer characterSpriteRenderer;
    protected Color regColor;

    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int maxMorale = 10;
    protected int health;
    protected int morale;
    protected bool available = true;
    protected bool firstCombat = true;

    private void Start()
    {
        SetUp();
    }

    protected void SetUp()
    {
        characterSpriteRenderer = GetComponent<SpriteRenderer>();
        regColor = characterSpriteRenderer.color;

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
        characterSpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.2f);
        characterSpriteRenderer.color = regColor;
    }

    // called at start of battle
    // use this method to reset things between fights
    virtual public void startBattle()
    {
        if (firstCombat)
        {
            health = maxHealth;
            morale = maxMorale;
            firstCombat = false;
        }

        available = true;

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
