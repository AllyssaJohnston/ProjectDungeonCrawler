using UnityEngine;
using System.Collections.Generic;

public class EnemyBehavior : CharacterBehavior
{
    public List<SpellBehavior> spellsToChooseFrom = new List<SpellBehavior>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        friendly = false;
        if (spellsToChooseFrom.Count == 0)
        {
            Debug.Log("no spells");
        }
    }

    // use this method to reset things between fights
    override public void startBattle()
    {
        
    }

    // use this method to reset things between turns
    override public void startTurn()
    {
        if (health > 0 && morale > 0)
        {
            available = true;
        }
        else
        {
            available = false;
        }
    }

    //chooses a spell and executes it
    public void chooseSpell(List<CharacterBehavior> friendlyCharacters)
    {
        if (canCast())
        {
            //TOOD choose a spell
            SpellBehavior spellBehavior = spellsToChooseFrom[0];

            Debug.Log("cast " + spellBehavior.spellName + " " + spellBehavior.damage + " damage, " + spellBehavior.moraleDamage + " morale ");
            
            // do morale damage against the enemy
            cast(spellBehavior.moraleDamage);

            //TODO choose how many characters to do damage against
            // do damage against the characters
            for (int i = 0; i < friendlyCharacters.Count; i++)
            {
                friendlyCharacters[i].updateHealth(-spellBehavior.damage);
            }
        }
    }
}
