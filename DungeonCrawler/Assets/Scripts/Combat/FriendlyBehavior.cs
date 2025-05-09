using UnityEngine;

public class FriendlyBehavior : CharacterBehavior
{
    // icon images
    public Sprite iconSprite;
    public Sprite iconUsedSprite;

    [SerializeField] int maxMorale = 10;
    protected int morale;

    override protected void SetUp()
    {
        morale = maxMorale;
        base.SetUp();
    }
    public int getMorale() { return morale; }

    public void updateMorale(int moraleChange)
    {
        morale += moraleChange;
        morale = Mathf.Max(morale, 0);
        available = isActive();
        UI_ManagerBehavior.UpdateMoraleBar();
    }

    public void setMorale(int givenMorale)
    {
        morale = givenMorale;
        UI_ManagerBehavior.UpdateMoraleBar();
    }

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

    // returns whether character is 'in' the fight
    override public bool isActive() { return health > 0 && morale > 0; }

    // take effects of casting a spell
    // and then makes the character unable to cast more spells
    public void cast(int moraleChange)
    {
        updateMorale(moraleChange);
        base.cast();
    }
}

