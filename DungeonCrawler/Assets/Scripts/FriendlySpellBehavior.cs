using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FriendlySpellBehavior : SpellBehavior
{
    public List<GameObject> castingCharacters = new List<GameObject>(); //up to 2

    public TMP_Text spellNameText;
    public TMP_Text damageText;
    public TMP_Text moraleDamageText;

    private void Start()
    {
        spellNameText.text = spellName;
        damageText.text = damage + " DAMAGE";
        moraleDamageText.text = moraleDamage + " MORALE DAMAGE";

        if (castingCharacters.Count == 0 )
        {
            Debug.Log("Invalid spell");
        }
    }
}
