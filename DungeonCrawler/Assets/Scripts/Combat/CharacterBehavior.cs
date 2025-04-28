using UnityEngine;

public class CharacterBehavior : MonoBehaviour
{
    public bool friendly = true;
    public int maxHealth = 100;
    protected int health;
    protected int maxMorale = 10;
    protected int morale;
    protected bool available = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        health = maxHealth;
        morale = maxMorale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public int getHealth()
    {
        return health;
    }

    public int getMorale()
    {
        return morale;
    }

    // updates the character's health
    public void updateHealth(int healthChange)
    {
        health += healthChange;
        if (health <= 0)
        {
            health = 0;
            Debug.Log("died");
            available = false;
        }
    }

    // use this method to reset things between fights
    virtual public void startBattle()
    {
        // recover whatever is greater: half of your missing morale rounded down OR 1
        int moraleDif = maxMorale - morale;
        morale += Mathf.Max(moraleDif / 2, 1);
        if (morale > maxMorale)
        {
            morale = maxMorale;
        }

        health = Mathf.Max(health, 1); // start with a minimum of 1 health
    }

    // use this method to reset things between turns
    virtual public void startTurn()
    {
        // set availability
        if (health > 0 && morale > 0)
        {
            available = true;
        }
        else
        {
            available = false;
        }
    }

    // returns whether character can cast spells
    public bool canCast()
    {
        return available;
    }

    // 'casts' a spell and then makes the character unable to cast more spells
    public void cast(int moraleChange)
    {
        morale += moraleChange;
        available = false;
    }
}
