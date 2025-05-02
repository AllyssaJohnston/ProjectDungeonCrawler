using UnityEngine;
using System.Collections.Generic;

public class EnemyBehavior : CharacterBehavior
{
    public List<EnemySpellBehavior> spellsToChooseFrom = new List<EnemySpellBehavior>();

    private int curSpellIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        friendly = false;
        if (spellsToChooseFrom.Count == 0)
        {
            Debug.Log("No spells");
        }
    }


    // use this method to reset things between fights
    override public void startBattle()
    {
        curSpellIndex = 0;
    }

    // use this method to reset things between turns
    override public void startTurn()
    {
        available = isActive();
    }

    //chooses a spell and executes it
    public void chooseSpell(List<CharacterBehavior> friendlyCharacters)
    {
        if (canCast())
        {
            castSpell(friendlyCharacters);
        }
    }

    private void castSpell(List<CharacterBehavior> friendlyCharacters)
    {
        EnemySpellBehavior spellBehavior = getSpell();

        string target = "all party members";
        if (spellBehavior.damageAllEnemies)
        {
            foreach (CharacterBehavior characterBehavior in friendlyCharacters)
            {
                characterBehavior.updateHealth(-spellBehavior.damage);
            }
        }
        else
        {
            //select character to target based on spell targeting data, and attack them
            CharacterBehavior characterBehavior = getCharacterTarget(getSpell().targeting, friendlyCharacters);
            characterBehavior.updateHealth(-spellBehavior.damage);
            target = characterBehavior.characterName;
        }
        DebugBehavior.updateLog(characterName + " cast " + spellBehavior.spellName + " on " + target + " for " + spellBehavior.damage + " damage.");
    }

    // rotate through spells
    private EnemySpellBehavior getSpell()
    {
        EnemySpellBehavior spellBehavior = spellsToChooseFrom[curSpellIndex];
        curSpellIndex++;
        if (curSpellIndex >= spellsToChooseFrom.Count)
        {
            curSpellIndex = 0;
        }
        return spellBehavior;
    }

    // choose target
    private CharacterBehavior getCharacterTarget(E_SPELL_TARGETING targeting, List<CharacterBehavior> friendlyCharacters)
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
}
