using UnityEngine.UI;
using UnityEngine;

public class CharacterIconBehavior : MonoBehaviour
{
    private Image iconImage;

    [SerializeField] Color unavailableColor = Color.black;

    public void SetUp(Sprite characterSprite)
    {
        iconImage = GetComponentInChildren<Image>();
        iconImage.sprite = characterSprite;
    }

    public void updateImage(bool canCast)
    {
        iconImage.color = canCast ? Color.white : unavailableColor;
    }
}
