using UnityEngine.UI;
using UnityEngine;

public class CharacterIconBehavior : MonoBehaviour
{
    private Image iconImage;
    private Sprite regSprite;
    private Sprite usedSprite;
    private Sprite deadSprite;

    public void SetUp(Sprite characterSprite, Sprite usedSprite, Sprite deadSprite)
    {
        iconImage = GetComponentInChildren<Image>();
        iconImage.sprite = characterSprite;
        regSprite = characterSprite;
        this.usedSprite = usedSprite;
        this.deadSprite = deadSprite;
    }

    public void updateImage(bool canCastSpell, bool canCharacterCast)
    {
        if (canCharacterCast == false)
        {
            iconImage.sprite = deadSprite;
        }
        else if (canCastSpell == false)
        {
            iconImage.sprite = usedSprite;
        }
        else //normal
        {
            iconImage.sprite = regSprite;
        }
    }
}
