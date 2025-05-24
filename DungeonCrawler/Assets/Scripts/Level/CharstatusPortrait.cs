using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharstatusPortrait : MonoBehaviour
{
    [SerializeField] int index = 0;
    [SerializeField] Image characterIcon;
    [SerializeField] public HealthBarManager healthBarManager;
    [SerializeField] public HealthBarManager moraleBarManager;

    private FriendlyBehavior character;
    private bool setUp = false;

    // Update is called once per frame
    void Update()
    {
        if (setUp)
        {
            healthBarManager.SetValue(character.getHealth());
            moraleBarManager.SetValue(character.getMorale());
        }
        else
        {
            setUpCharacterUI();
        }
    }

    private void setUpCharacterUI() 
    {
         
        if (CombatManagerBehavior.friendlyCharacterBehaviors.Count == 0) return;
        character = CombatManagerBehavior.friendlyCharacterBehaviors[index];
        character.SetUp();
        characterIcon.sprite = character.iconSprite;
        healthBarManager.SetUp(character.getMaxHealth(), Color.red, Color.white);
        moraleBarManager.SetUp(character.getMaxMorale(), Color.white, Color.black);
        setUp = true;
    }
}
