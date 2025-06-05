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
                foreach (FriendlyBehavior character in CombatManagerBehavior.friendlyCharacterBehaviors)
                {
                    character.updateHealth(amount);
                }

                break;
            case PotionType.Morale:
                Debug.Log($"Restored {amount} MP.");
                foreach (FriendlyBehavior character in CombatManagerBehavior.friendlyCharacterBehaviors)
                {
                    character.updateMorale(amount);
                }
                break;
        }
    }
}