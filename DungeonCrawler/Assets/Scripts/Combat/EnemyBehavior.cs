using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemyBehavior : CharacterBehavior
{
    [SerializeField] List<EnemySpellStats> spellsToChooseFrom = new List<EnemySpellStats>();

    private int curSpellIndex = 0;


    // use this method to reset things between fights
    override public void startBattle()
    {
        if (firstCombat)
        {
            health = maxHealth;
            firstCombat = false;
        }

        available = true;
        curSpellIndex = 0;
        SetUp();
    }

    override protected void SetUp()
    {
        base.SetUp();
        friendly = false;
        if (spellsToChooseFrom.Count == 0)
        {
            Debug.Log("No spells");
        }

    }

    // use this method to reset things between turns
    override public void startTurn()
    {
        available = isActive();
        characterImageManager.sprite = available ? regSprite : usedSprite;
    }

    //chooses a spell and executes it
    public void castSpell(List<FriendlyBehavior> friendlyCharacters)
    {
        if (canCast())
        {
            EnemySpellStats spell = getSpell();

            string target = "all party members";
            if (spell.damageAllEnemies)
            {
                foreach (CharacterBehavior characterBehavior in friendlyCharacters)
                {
                    characterBehavior.updateHealth(-spell.damage);
                }
            }
            else
            {
                //select character to target based on spell targeting data, and attack them
                FriendlyBehavior characterBehavior = getCharacterTarget(getSpell().targeting, friendlyCharacters);
                characterBehavior.updateHealth(-spell.damage);
                target = characterBehavior.characterName;
            }
            DebugBehavior.updateLog(characterName + " cast " + spell.spellName + " on " + target + " for " + spell.damage + " damage.");
            available = false;
            //characterImageManager.sprite = usedSprite;
        }
        else
        {
            DebugBehavior.updateLog(characterName + " can't cast!");
        }
    }

    // rotate through spells
    private EnemySpellStats getSpell()
    {
        EnemySpellStats spellStat = spellsToChooseFrom[curSpellIndex];
        curSpellIndex++;
        if (curSpellIndex >= spellsToChooseFrom.Count)
        {
            curSpellIndex = 0;
        }
        return spellStat;
    }

    // choose target
    private FriendlyBehavior getCharacterTarget(E_SPELL_TARGETING targeting, List<FriendlyBehavior> friendlyCharacters)
    {
        int highestHealthIndex = 0;
        int lowestHealthIndex = 0;
        int highestMoraleIndex = 0;
        int lowestMoraleIndex = 0;

        for (int i = 1; i < friendlyCharacters.Count; i++)
        {
            if (friendlyCharacters[i].getHealth() >= friendlyCharacters[highestHealthIndex].getHealth())
            {
                highestHealthIndex = i;
            }
            if (friendlyCharacters[i].getHealth() <= friendlyCharacters[lowestHealthIndex].getHealth())
            {
                lowestHealthIndex = i;
            }
            if (friendlyCharacters[i].getMorale() >= friendlyCharacters[highestMoraleIndex].getMorale())
            {
                highestMoraleIndex = i;
            }
            if (friendlyCharacters[i].getMorale() <= friendlyCharacters[lowestMoraleIndex].getMorale())
            {
                lowestMoraleIndex = i;
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

    public void setUpFromStats(EnemyStats stats)
    {
        characterName = stats.characterName;
        regSprite = stats.characterSprite;
        GetComponent<Image>().sprite = stats.characterSprite;
        damagedSprite = stats.characterDamagedSprite;
        usedSprite = stats.characterUsedSprite;
        
        maxHealth = stats.maxHealth;

        spellsToChooseFrom = stats.spells;
    }
}
