using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBehavior : MonoBehaviour
{
    public bool friendly = true;
    public string characterName = "unnamed";

    protected Sprite regSprite;
    public Sprite damagedSprite;
    public Sprite usedSprite;

    protected Image characterImageManager;

    [SerializeField] protected int maxHealth = 100;
    protected int health;
    protected bool available = true;
    protected bool firstCombat = true;

    protected CharacterUICreatorBehavior UI_ManagerBehavior;

    // called at the start of first battle
    virtual protected void SetUp()
    {
        characterImageManager = GetComponent<Image>();
        regSprite = characterImageManager.sprite;

        health = maxHealth;

        UI_ManagerBehavior = gameObject.GetComponent<CharacterUICreatorBehavior>();
        UI_ManagerBehavior.SetUp(this);
    }

    public int getHealth() { return health; }

    // updates the character's health
    public void updateHealth(int healthChange)
    {
        health += healthChange;
        health = Mathf.Max(health, 0);
        available = isActive();
        UI_ManagerBehavior.UpdateHealthBar();

        if (healthChange < 0)
        {
            StartCoroutine(takeDamageEffect());
        }
    }

    public void setHealth(int givenHealth)
    {
        health = givenHealth;
        UI_ManagerBehavior.UpdateHealthBar();
    }

    public IEnumerator takeDamageEffect()
    {
        characterImageManager.color = Color.red;
        characterImageManager.sprite = damagedSprite;
        yield return new WaitForSeconds(.2f);
        characterImageManager.color = Color.white;
        characterImageManager.sprite = available ? regSprite : usedSprite;
    }

    // called after failed combat
    virtual public void reset()
    {
        health = maxHealth;
        characterImageManager.color = Color.white;
        characterImageManager.sprite = regSprite;
        available = true;
        firstCombat = true;
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

    // returns whether character is still alive in the fight
    virtual public bool isActive() { return health > 0; }

    // Sets the character unable to cast more spells for this turn
    virtual public void cast()
    {
        available = false;
        characterImageManager.sprite = usedSprite;
    }
}
