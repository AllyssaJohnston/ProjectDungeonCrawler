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

    void setUpCharacterUI() 
    {
        List<GameObject> party = new List<GameObject>();
        party = CombatManagerBehavior.getParty();
        if (party == null) return;
        character = party[index].GetComponent<FriendlyBehavior>();
        character.SetUp();
        characterIcon.sprite = character.iconSprite;
        healthBarManager.SetUp(character.getHealth(), Color.red, Color.white);
        moraleBarManager.SetUp(character.getMorale(), Color.white, Color.black);
        setUp = true;
    }
}
