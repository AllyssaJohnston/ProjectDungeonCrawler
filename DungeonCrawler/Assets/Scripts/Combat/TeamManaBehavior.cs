using TMPro;
using UnityEngine;

public class TeamManaBehavior : MonoBehaviour
{
    public GameManagerBehavior gameManager;
    public TMP_Text manaText;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        manaText.text = gameManager.getMana().ToString();
    }
}
