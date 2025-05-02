using UnityEngine;

public class CharacterUICreatorBehavior : MonoBehaviour
{
    [SerializeField] public GameObject healthBarTemplate;
    public Color healthColor;
    public Color moraleColor;
    public float UI_OffsetX = 0;
    public float healthPosY = 3;
    public float moralePosY = 2;
    public bool includeMorale = true;
    [SerializeField] public GameObject panel;
    private CharacterBehavior characterBehavior;

    private GameObject healthBarManager;
    private HealthBarManager healthBarManagerBehavior;
    private GameObject moraleBarManager;
    private HealthBarManager moraleBarManagerBehavior;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterBehavior = gameObject.GetComponent<CharacterBehavior>();

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
        bar = Instantiate(healthBarTemplate);
        Vector3 scale = bar.transform.localScale;
        bar.transform.SetParent(panel.transform);
        bar.transform.localScale = scale;
        bar.transform.position = new Vector3(gameObject.transform.position.x + UI_OffsetX, yOffset, 0);
        return bar;
    }

    // Update is called once per frame
    void Update()
    {
        healthBarManagerBehavior.SetValue(characterBehavior.getHealth());
        if (includeMorale)
        {
            moraleBarManagerBehavior.SetValue(characterBehavior.getMorale());
        }
    }
}
