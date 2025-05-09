using UnityEngine;

public class CharacterUICreatorBehavior : MonoBehaviour
{
    [SerializeField] GameObject healthBarTemplate;
    [SerializeField] Color healthBarColor;
    [SerializeField] Color healthTextColor;
    [SerializeField] Color moraleBarColor;
    [SerializeField] Color moraleTextColor;
    [SerializeField] float UI_OffsetX = 0;
    [SerializeField] float healthPosY = 3;
    [SerializeField] float moralePosY = 2;

    private CharacterBehavior characterBehavior;
    private RectTransform characterRect;
    private GameObject healthBarManager;
    private HealthBarManager healthBarManagerBehavior;
    private GameObject moraleBarManager;
    private HealthBarManager moraleBarManagerBehavior;

    public void SetUp(CharacterBehavior givenCharacterBehavior)
    {
        characterBehavior = givenCharacterBehavior;
        characterRect = gameObject.transform.parent.gameObject.GetComponent<RectTransform>();

        healthBarManager = createBar(healthPosY);
        healthBarManagerBehavior = healthBarManager.GetComponent<HealthBarManager>();
        healthBarManagerBehavior.SetUp(characterBehavior.getHealth(), healthBarColor, healthTextColor);

        if (givenCharacterBehavior.friendly)
        {
            moraleBarManager = createBar(moralePosY);
            moraleBarManagerBehavior = moraleBarManager.GetComponent<HealthBarManager>();
            moraleBarManagerBehavior.SetUp(((FriendlyBehavior)characterBehavior).getMorale(), moraleBarColor, moraleTextColor);

        }
    }

    private GameObject createBar(float yOffset)
    {
        GameObject bar = Instantiate(healthBarTemplate);
        RectTransform rectTransform = bar.GetComponent<RectTransform>();
        Vector3 scale = bar.transform.localScale;
        bar.transform.SetParent(gameObject.transform.parent);
        bar.transform.localScale = scale;

        Vector3 center = characterRect.TransformPoint(characterRect.anchoredPosition);
        rectTransform.anchoredPosition = new Vector3(UI_OffsetX, yOffset, 0);
        return bar;
    }

    public void UpdateHealthBar()
    {
        healthBarManagerBehavior.SetValue(characterBehavior.getHealth());
    }

    public void UpdateMoraleBar()
    {
        if (characterBehavior.friendly)
        {
            moraleBarManagerBehavior.SetValue(((FriendlyBehavior)characterBehavior).getMorale());
        }
    }
}
