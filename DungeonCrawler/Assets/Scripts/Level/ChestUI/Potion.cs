using UnityEngine;


[CreateAssetMenu(fileName = "New Potion", menuName = "Items/Potion")]
public class Potion : Item
{
    public enum PotionType { Health, Morale }
    public PotionType type;
    public int amount;

    public override void Use()
    {
        switch (type)
        {
            case PotionType.Health:
                Debug.Log($"Restored {amount} HP.");
                Debug.Log($"Character list count: {CombatManagerBehavior.friendlyCharacterBehaviors.Count}");
                foreach (FriendlyBehavior character in CombatManagerBehavior.friendlyCharacterBehaviors)
                {
                    Debug.Log("For each char");
                    character.updateHealth(amount);
                    GameObject charstatusPanel = GameObject.Find("CharstatusPanel");
                    if (charstatusPanel != null)
                    {
                        Debug.Log("Found character portrait ui");
                        foreach (Transform characterTransform in charstatusPanel.transform)
                        {
                            Debug.Log("Updating character portrait ui");
                            characterTransform.GetComponent<CharstatusPortrait>().healthBarManager.SetValue(character.getHealth());
                        }
                    }
                    else
                    {
                        Debug.LogError("CharstatusPanel not found!");
                    }

                }

                break;
            case PotionType.Morale:
                Debug.Log($"Restored {amount} MP.");
                foreach (FriendlyBehavior character in CombatManagerBehavior.friendlyCharacterBehaviors)
                {
                    Debug.Log("For each char");
                    character.updateMorale(amount);
                    GameObject charstatusPanel = GameObject.Find("CharstatusPanel");
                    if (charstatusPanel != null)
                    {
                        Debug.Log("Found character portrait ui");
                        foreach (Transform characterTransform in charstatusPanel.transform)
                        {
                            Debug.Log("Updating character portrait ui");
                            characterTransform.GetComponent<CharstatusPortrait>().moraleBarManager.SetValue(character.getMorale());
                        }
                    }
                    else
                    {
                        Debug.LogError("CharstatusPanel not found!");
                    }

                }
                break;
        }
    }
}