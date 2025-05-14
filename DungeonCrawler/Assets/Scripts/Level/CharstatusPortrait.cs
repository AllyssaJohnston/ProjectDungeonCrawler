using UnityEngine;
using System.Collections.Generic;

public class CharstatusPortrait : MonoBehaviour
{
    public int index = 0;
    private List<GameObject> party = null;
    private UnityEngine.UI.Image image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = this.GetComponent<UnityEngine.UI.Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (party == null) setupParty();
    }

    void setupParty() 
    {
        party = CombatManagerBehavior.getParty();
        if (party == null) return;
        image.sprite = party[index].GetComponent<FriendlyBehavior>().iconSprite;
    }
}
