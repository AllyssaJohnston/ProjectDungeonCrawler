using TMPro;
using UnityEngine;

public class CharacterStatBehavior : MonoBehaviour
{
    public CharacterBehavior character;
    public TMP_Text healthText;
    public TMP_Text moraleText;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = character.getHealth().ToString();
        moraleText.text = character.getMorale().ToString();
    }
}
