using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBehavior : MonoBehaviour
{
    protected bool friendly = true;
    public string characterName = "unnamed";
    // alt character images
    protected Sprite regSprite;
    public Sprite damagedSprite;
    public Sprite usedSprite;
    // icon images
    public Sprite iconSprite;
    public Sprite iconUsedSprite;

    protected Image characterImageManager;
    protected Color regColor;

    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int maxMorale = 10;
    protected int health;
    protected int morale;
    protected bool available = true;
    protected bool firstCombat = true;

    private CharacterUICreatorBehavior UI_ManagerBehavior;

    virtual protected void SetUp()
    {
        characterImageManager = GetComponent<Image>();
        regSprite = characterImageManager.sprite;
        regColor = characterImageManager.color;

        health = maxHealth;
        morale = maxMorale;

        UI_ManagerBehavior = gameObject.GetComponent<CharacterUICreatorBehavior>();
        UI_ManagerBehavior.SetUp(this);
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
        UI_ManagerBehavior.UpdateHealthBar();
    }

    public void setHealth(int givenHealth)
    {
        health = givenHealth;
        UI_ManagerBehavior.UpdateHealthBar();
    }

    public void updateMorale(int moraleChange)
    {
        morale += moraleChange;
        if (morale <= 0)
        {
            morale = 0;
            available = false;
        }
        UI_ManagerBehavior.UpdateMoraleBar();
    }

    public void setMorale(int givenMorale)
    {
        morale = givenMorale;
        UI_ManagerBehavior.UpdateMoraleBar();
    }

    public IEnumerator takeDamageEffect()
    {
        characterImageManager.color = Color.red;
        characterImageManager.sprite = damagedSprite;
        yield return new WaitForSeconds(.2f);
        characterImageManager.color = regColor;
        characterImageManager.sprite = regSprite;
    }

    // called at start of battle
    // use this method to reset things between fights
    virtual public void startBattle()
    {
        // start gets called AFTER startBattle, so do setup here
        if (firstCombat)
        {
            SetUp();
            firstCombat = false;
        }

        available = true;

        // morale regen
        // recover whatever is greater: half of your missing morale rounded down OR 1
        int moraleDif = maxMorale - morale;
        updateMorale(Mathf.Max(moraleDif / 2, 1));
        setMorale(Mathf.Min(morale, maxMorale));

        // health regen
        // start with a minimum of 1 health
        setHealth(Mathf.Max(health, 1));
    }

    // called at start of turn
    // use this method to reset things between turns
    virtual public void startTurn()
    {
        // set availability
        available = isActive();
        characterImageManager.sprite = available ? regSprite : usedSprite;
    }

    // returns whether character can cast spells
    public bool canCast() { return available; }

    // returns whether character is 'in' the fight
    public bool isActive() { return health > 0 && morale > 0; }

    // take effects of casting a spell
    // and then makes the character unable to cast more spells
    public void cast(int moraleChange)
    {
        updateMorale(moraleChange);
        available = false;
        characterImageManager.sprite = usedSprite;
    }
}
