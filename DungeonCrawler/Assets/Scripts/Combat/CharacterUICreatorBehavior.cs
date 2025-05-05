using UnityEngine;

public class CharacterUICreatorBehavior : MonoBehaviour
{
    [SerializeField] GameObject healthBarTemplate;
    [SerializeField] Color healthColor;
    [SerializeField] Color moraleColor;
    [SerializeField] float UI_OffsetX = 0;
    [SerializeField] float healthPosY = 3;
    [SerializeField] float moralePosY = 2;
    [SerializeField] bool includeMorale = true;
    [SerializeField] GameObject panel;

    private CharacterBehavior characterBehavior;
    private GameObject healthBarManager;
    private HealthBarManager healthBarManagerBehavior;
    private GameObject moraleBarManager;
    private HealthBarManager moraleBarManagerBehavior;

    public void SetUp(CharacterBehavior givenCharacterBehavior)
    {
        characterBehavior = givenCharacterBehavior;

        healthBarManager = createBar(healthPosY);
        healthBarManagerBehavior = healthBarManager.GetComponent<HealthBarManager>();
        healthBarManagerBehavior.SetUp(characterBehavior.getHealth(), healthColor);

        if (includeMorale)
        {
            moraleBarManager = createBar(moralePosY);
            moraleBarManagerBehavior = moraleBarManager.GetComponent<HealthBarManager>();
            moraleBarManagerBehavior.SetUp(characterBehavior.getMorale(), moraleColor);

        }
    }

    private GameObject createBar(float yOffset)
    {
        GameObject bar = Instantiate(healthBarTemplate);
        Vector3 scale = bar.transform.localScale;
        bar.transform.SetParent(panel.transform);
        bar.transform.localScale = scale;

        bar.transform.position = new Vector3(gameObject.transform.position.x + UI_OffsetX, yOffset, 0);
        return bar;
    }

    public void UpdateHealthBar()
    {
        healthBarManagerBehavior.SetValue(characterBehavior.getHealth());
    }

    public void UpdateMoraleBar()
    {
        if (includeMorale)
        {
            moraleBarManagerBehavior.SetValue(characterBehavior.getMorale());
        }
    }
}
