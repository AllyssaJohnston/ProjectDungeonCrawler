using UnityEngine;

public class FriendlyBehavior : CharacterBehavior
{

    // icon images
    public Sprite iconSprite;
    public Sprite iconUsedSprite;
    public Sprite iconDeadSprite;

    [SerializeField] int maxMorale = 10;
    protected int morale;

    // called at the start of the first battle
    override public void SetUp()
    {
        if (!setUp)
        {
            morale = maxMorale;
            base.SetUp();
        }
    }

    public int getMaxMorale() { return maxMorale; }

    public int getMorale() { return morale; }

    public void updateMorale(int moraleChange)
    {
        morale += moraleChange;
        morale = Mathf.Max(morale, 0);
        morale = Mathf.Min(morale, maxMorale);
        UI_ManagerBehavior.UpdateMoraleBar();
    }

    public void setMorale(int givenMorale)
    {
        morale = givenMorale;
        UI_ManagerBehavior.UpdateMoraleBar();
    }

    // called after failed combat
    override public void reset()
    {
        base.reset();
        morale = maxMorale;
    }

    // called at start of battle
    // use this method to reset things between fights
    override public void startBattle()
    {
        base.startBattle();
        
        // morale regen
        // recover whatever is greater: half of your missing morale rounded down OR 1
        int moraleDif = maxMorale - morale;
        updateMorale(Mathf.Max(moraleDif / 2, 1));
        setMorale(Mathf.Min(morale, maxMorale));
    }

    // returns whether character is still alive in the fight
    override public bool isAlive() { return health > 0 && morale > 0; }

    // revive with 50% health and 2 morale
    public void revive()
    {
        setHealth(maxHealth / 2);
        setMorale(Mathf.Max(morale, 2));
        setSprite();
    }

    // overloaded method from character behavior
    // Sets the character unable to cast more spells
    public void cast(int moraleChange)
    {
        updateMorale(moraleChange);
        base.cast();
    }
}

