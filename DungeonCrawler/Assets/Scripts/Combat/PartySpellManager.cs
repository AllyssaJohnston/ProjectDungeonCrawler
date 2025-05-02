using UnityEngine;
using System.Collections.Generic;

public class PartySpellManager : MonoBehaviour
{
    [SerializeField] float verticalSpacing;

    FriendlySpellBehavior[] spells;
    int numCastable = 0;
    int numUncastable = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spells = GetComponentsInChildren<FriendlySpellBehavior>();
        UpdateSpellOrder();
    }

    private void Update()
    {
        UpdateSpellOrder();
    }

    private void UpdateSpellOrder()
    {
        numCastable = 0;
        numUncastable = 0;
        foreach (FriendlySpellBehavior spell in spells)
        {
            if (spell.updateAndCanCast())
            {
                spell.gameObject.transform.SetSiblingIndex(numCastable);
                numCastable++;
            }
            else
            {
                spell.gameObject.transform.SetSiblingIndex(numCastable + numUncastable);
                numUncastable++;
            }
        }
    }

}
