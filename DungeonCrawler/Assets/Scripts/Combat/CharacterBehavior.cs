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
    protected bool castThisTurn = false;
    protected bool setUp = false;

    protected CharacterUICreatorBehavior UI_ManagerBehavior;

    // called at the start of first battle
    virtual public void SetUp()
    {
        characterImageManager = GetComponent<Image>();
        regSprite = characterImageManager.sprite;

        health = maxHealth;

        UI_ManagerBehavior = gameObject.GetComponent<CharacterUICreatorBehavior>();
        UI_ManagerBehavior.SetUp(this);
        setUp = true;
    }

    public int getHealth() { return health; }

    // updates the character's health
    public void updateHealth(int healthChange)
    {
        health += healthChange;
        health = Mathf.Max(health, 0);
        health = Mathf.Min(health, maxHealth);
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
        characterImageManager.sprite = canCast() ? regSprite : usedSprite;
    }

    // called after failed combat
    virtual public void reset()
    {
        health = maxHealth;
        characterImageManager.color = Color.white;
        characterImageManager.sprite = regSprite;
        castThisTurn = false;
        setUp = false;
    }

    // called at start of battle
    // use this method to reset things between fights
    virtual public void startBattle()
    {
        // start gets called AFTER startBattle, so do setup here
        if (!setUp)
        {
            SetUp();
        }

        castThisTurn = false;

        // health regen
        // start with a minimum of 1 health
        setHealth(Mathf.Max(health, 1));
    }

    // called at start of turn
    // use this method to reset things between turns
    virtual public void startTurn()
    {
        // set availability
        castThisTurn = false;
        characterImageManager.sprite = canCast() ? regSprite : usedSprite;
    }

    // returns whether character can cast spells
    public bool canCast() { return isAlive() && !castThisTurn; }

    // returns whether character is still alive in the fight
    virtual public bool isAlive() { return health > 0; }

    // Sets the character unable to cast more spells for this turn
    virtual public void cast()
    {
        castThisTurn = true;
        characterImageManager.sprite = usedSprite;
    }
}
