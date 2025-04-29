using TMPro;
using UnityEngine;

public class CharacterStatBehavior : MonoBehaviour
{
    public TMP_Text healthText;
    public TMP_Text moraleText;

    public void updateText(int health, int morale)
    {
        healthText.text = health.ToString();
        moraleText.text = morale.ToString();
    }
}
