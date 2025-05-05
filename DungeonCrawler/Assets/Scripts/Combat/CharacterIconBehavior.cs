using UnityEngine.UI;
using UnityEngine;

public class CharacterIconBehavior : MonoBehaviour
{
    private Image iconImage;
    private Sprite regSprite;
    private Sprite usedSprite;

    //[SerializeField] Color unavailableColor = Color.black;

    public void SetUp(Sprite characterSprite, Sprite usedSprite)
    {
        iconImage = GetComponentInChildren<Image>();
        iconImage.sprite = characterSprite;
        regSprite = characterSprite;
        this.usedSprite = usedSprite;
    }

    public void updateImage(bool canCast)
    {
        iconImage.sprite = canCast ? regSprite : usedSprite;
        //iconImage.color = canCast ? Color.white : unavailableColor;
    }
}
