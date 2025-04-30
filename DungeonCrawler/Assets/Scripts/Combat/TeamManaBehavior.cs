using TMPro;
using UnityEngine;

public class TeamManaBehavior : MonoBehaviour
{
    public TMP_Text manaText;

    // Update is called once per frame
    void Update()
    {
        manaText.text = CombatManagerBehavior.getMana().ToString();
    }
}
