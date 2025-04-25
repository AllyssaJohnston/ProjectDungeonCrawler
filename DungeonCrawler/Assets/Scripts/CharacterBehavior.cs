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
        if(health < 0)
        {
            //TODO die
            Debug.Log("health below 0");
            available = false;
        }
    }

    // use this method to reset things between fights
    virtual public void startBattle()
    {
        
    }

    // use this method to reset things between turns
    virtual public void startTurn()
    {
        if (health > 0 && morale > 0)
        {
            available = true;
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
