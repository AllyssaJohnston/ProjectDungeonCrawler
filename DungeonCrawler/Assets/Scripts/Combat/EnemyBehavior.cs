using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyBehavior : CharacterBehavior
{
    public List<SpellBehavior> spellsToChooseFrom = new List<SpellBehavior>();
    private bool willCastSpell = false;
    private List<CharacterBehavior> possibleTargets = new List<CharacterBehavior>();
    private float spellCastTimer = 0f;
    private float waitTime = 3f;
    private StateManagerBehavior stateManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stateManager = FindFirstObjectByType<StateManagerBehavior>();
        friendly = false;
        if (spellsToChooseFrom.Count == 0)
        {
            Debug.Log("no spells");
        }
    }

    private void Update()
    {
        if (willCastSpell)
        {
            spellCastTimer += Time.deltaTime;
            if (spellCastTimer > waitTime)
            {
                castSpell();
            }
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
        if (!willCastSpell)
        {
            Debug.Log("Enemy choosing spell");
            willCastSpell = true;
            possibleTargets = friendlyCharacters;
        }
    }

    public void castSpell()
    {
        Debug.Log("enemy casting spell");
        if (canCast())
        {
            //TOOD choose a spell
            SpellBehavior spellBehavior = spellsToChooseFrom[0];

            Debug.Log("cast " + spellBehavior.spellName + " " + spellBehavior.damage + " damage, " + spellBehavior.moraleDamage + " morale ");

            // do morale damage against the enemy
            cast(spellBehavior.moraleDamage);

            if (spellBehavior.damageAllEnemies)
            {
                for (int i = 0; i < possibleTargets.Count; i++)
                {
                    possibleTargets[i].updateHealth(-spellBehavior.damage);
                }
            }
            else
            {
                // TODO choose which character to do damage against
                possibleTargets[0].updateHealth(-spellBehavior.damage);
            }
        }
        willCastSpell = false;
        spellCastTimer = 0f;
        stateManager.NextState();
    }
}
