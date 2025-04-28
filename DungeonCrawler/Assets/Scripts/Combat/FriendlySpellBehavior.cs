using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FriendlySpellBehavior : SpellBehavior
{
    [SerializeField] public List<GameObject> castingCharacters = new List<GameObject>(); //up to 2

    [SerializeField] public TMP_Text spellNameText;
    [SerializeField] public TMP_Text damageText;
    [SerializeField] public TMP_Text moraleDamageText;
    [SerializeField] public TMP_Text manaText;
    [SerializeField] public TMP_Text targetingText;

    private void Start()
    {
        spellNameText.text = spellName;
        damageText.text = damage + " DAMAGE";
        moraleDamageText.text = moraleDamage + " MORALE DAMAGE";
        manaText.text = manaCost.ToString();
        targetingText.text = "TARGET " + (damageAllEnemies? "ALL" : "SELECT");

        if (castingCharacters.Count == 0 )
        {
            Debug.Log("Invalid spell");
        }
    }
}
