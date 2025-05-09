using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBehavior : MonoBehaviour
{
    public bool friendly = true;
    public string characterName = "unnamed";
    // alt character images
    public Sprite regSprite;
    public Sprite damagedSprite;
    public Sprite usedSprite;

    protected Image characterImageManager;
    protected Color regColor;

    [SerializeField] protected int maxHealth = 100;
    protected int health;
    protected bool available = true;
    protected bool firstCombat = true;

    protected CharacterUICreatorBehavior UI_ManagerBehavior;

    virtual protected void SetUp()
    {
        characterImageManager = GetComponent<Image>();
        regSprite = characterImageManager.sprite;
        regColor = characterImageManager.color;

        health = maxHealth;

        UI_ManagerBehavior = gameObject.GetComponent<CharacterUICreatorBehavior>();
        UI_ManagerBehavior.SetUp(this);
    }

    public int getHealth() { return health; }

    // updates the character's health
    public void updateHealth(int healthChange)
    {
        if (healthChange < 0)
        {
            StartCoroutine(takeDamageEffect());
        }

        health += healthChange;
        health = Mathf.Max(health, 0);
        available = isActive();
        UI_ManagerBehavior.UpdateHealthBar();
    }

    public void setHealth(int givenHealth)
    {
        health = givenHealth;
        UI_ManagerBehavior.UpdateHealthBar();
    }

    public IEnumerator takeDamageEffect()
    {
        Color curColor = characterImageManager.color;
        Sprite curSprite = characterImageManager.sprite;
        characterImageManager.color = Color.red;
        characterImageManager.sprite = damagedSprite;
        yield return new WaitForSeconds(.2f);
        characterImageManager.color = curColor;
        characterImageManager.sprite = curSprite;
    }

    // called at start of battle
    // use this method to reset things between fights
    virtual public void startBattle()
    {
        // start gets called AFTER startBattle, so do setup here
        if (firstCombat)
        {
            this.SetUp();
            firstCombat = false;
        }

        available = true;

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
    virtual public bool isActive() { return health > 0; }

    // take effects of casting a spell
    // and then makes the character unable to cast more spells
    virtual public void cast()
    {
        available = false;
        characterImageManager.sprite = usedSprite;
    }
}
