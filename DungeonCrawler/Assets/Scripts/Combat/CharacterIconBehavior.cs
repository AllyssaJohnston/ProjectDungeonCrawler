using UnityEngine.UI;
using UnityEngine;

public class CharacterIconBehavior : MonoBehaviour
{
    public void SetUp(Sprite characterSprite)
    {
        GetComponentInChildren<Image>().sprite = characterSprite;
    }

    public void updateImage(bool canCast)
    {
        if (canCast)
        {
            //character is available
            GetComponentInChildren<Image>().color = Color.white;
        }
        else
        {
            GetComponentInChildren<Image>().color = Color.red;
        }
    }
}
