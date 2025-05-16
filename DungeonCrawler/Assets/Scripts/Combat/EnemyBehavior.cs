using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemyBehavior : CharacterBehavior
{
    [SerializeField] List<EnemySpellStats> spellsToChooseFrom = new List<EnemySpellStats>();
    private int curSpellIndex = 0;


    override public void SetUp()
    {
        base.SetUp();
        friendly = false;
        if (spellsToChooseFrom.Count == 0)
        {
            Debug.Log("No spells");
        }
        foreach (EnemySpellStats spell in spellsToChooseFrom)
        {
            spell.setUpStringStats();
        }

    }

    public override void reset()
    {
        base.reset();
        curSpellIndex = 0;
    }

    public override void startBattle()
    {
        base.startBattle();
        curSpellIndex = 0;
    }

    // rotate through spells
    public EnemySpellStats getSpell()
    {
        EnemySpellStats spellStat = spellsToChooseFrom[curSpellIndex];
        // increment for next turn
        curSpellIndex++;
        if (curSpellIndex >= spellsToChooseFrom.Count)
        {
            curSpellIndex = 0;
        }
        return spellStat;
    }

    // choose target
    public FriendlyBehavior getCharacterTarget(E_SPELL_TARGETING targeting, List<FriendlyBehavior> friendlyCharacters)
    {
        int highestHealthIndex = 0;
        int lowestHealthIndex = 0;
        int highestMoraleIndex = 0;
        int lowestMoraleIndex = 0;
        bool firstCharAlive = friendlyCharacters[0].isAlive();

        for (int i = 1; i < friendlyCharacters.Count; i++)
        {
            if (friendlyCharacters[i].isAlive())
            {
                if ((friendlyCharacters[i].getHealth() >= friendlyCharacters[highestHealthIndex].getHealth()) || !firstCharAlive)
                {
                    highestHealthIndex = i;
                }
                if ((friendlyCharacters[i].getHealth() <= friendlyCharacters[lowestHealthIndex].getHealth()) || !firstCharAlive)
                {
                    lowestHealthIndex = i;
                }
                if ((friendlyCharacters[i].getMorale() >= friendlyCharacters[highestMoraleIndex].getMorale()) || !firstCharAlive)
                {
                    highestMoraleIndex = i;
                }
                if ((friendlyCharacters[i].getMorale() <= friendlyCharacters[lowestMoraleIndex].getMorale()) || !firstCharAlive)
                {
                    lowestMoraleIndex = i;
                }
            }
        }
        switch (targeting)
        {
            case E_SPELL_TARGETING.HIGHEST_HEALTH:
                return friendlyCharacters[highestHealthIndex];
            case E_SPELL_TARGETING.LOWEST_HEALTH:
                return friendlyCharacters[lowestHealthIndex];
            case E_SPELL_TARGETING.HIGHEST_MORALE:
                return friendlyCharacters[highestMoraleIndex];
            case E_SPELL_TARGETING.LOWEST_MORALE:
                return friendlyCharacters[lowestMoraleIndex];
            default:
                Debug.Log("invalid spell targeting type");
                break;
        }

        return friendlyCharacters[0];
    }

    // Called at start of battle to set up the enemy character
    public void setUpFromStats(EnemyStats stats)
    {
        characterName = stats.characterName;

        GetComponent<Image>().sprite = stats.characterSprite;
        regSprite = stats.characterSprite;
        damagedSprite = stats.characterDamagedSprite;
        usedSprite = stats.characterUsedSprite;
        deadSprite = stats.characterDeadSprite;

        RectTransform rect = GetComponent<RectTransform>();
        float anchorX = rect.anchoredPosition.x;
        float anchorY = rect.anchoredPosition.y;
        rect.offsetMax = new Vector2(stats.imageWidth, stats.imageHeight - 80) / 2f;
        rect.anchoredPosition = new Vector2(anchorX, anchorY);

        maxHealth = stats.maxHealth;

        spellsToChooseFrom = stats.spells;
        foreach (EnemySpellStats spell in spellsToChooseFrom)
        {
            spell.setUpStringStats();
        }
    }
}
