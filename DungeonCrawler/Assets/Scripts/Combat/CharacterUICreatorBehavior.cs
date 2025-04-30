using UnityEngine;

public class CharacterUICreatorBehavior : MonoBehaviour
{
    [SerializeField] public GameObject characterStatTemplate;
    [SerializeField] public GameObject characterIconTemplate;
    [SerializeField] public GameObject panel;
    private CharacterBehavior characterBehavior;
    private GameObject characterStat;
    private CharacterStatBehavior characterStatBehavior;
    private GameObject characterIcon;
    private CharacterIconBehavior characterIconBehavior;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterBehavior = gameObject.GetComponent<CharacterBehavior>();

        characterStat = Instantiate(characterStatTemplate);
        Vector3 scale = characterStat.transform.localScale;
        characterStat.transform.SetParent(panel.transform);
        characterStat.transform.localScale = scale;
        float posY = 2;
        characterStat.transform.position = new Vector3(gameObject.transform.position.x + 1.7f, posY, 0);
        characterStatBehavior = characterStat.GetComponent<CharacterStatBehavior>();


        characterIcon = Instantiate(characterIconTemplate);
        Vector3 scale2 = characterIcon.transform.localScale;
        characterIcon.transform.SetParent(panel.transform);
        characterIcon.transform.localScale = scale2;
        float posY2 = -2.3f;
        characterIcon.transform.position = new Vector3(gameObject.transform.position.x, posY2, 0);
        characterIconBehavior = characterIcon.GetComponent<CharacterIconBehavior>();
        characterIconBehavior.SetUp(characterBehavior.iconSprite);
    }

    // Update is called once per frame
    void Update()
    {
        characterStatBehavior.updateText(characterBehavior.getHealth(), characterBehavior.getMorale());
        characterIconBehavior.updateImage(characterBehavior.canCast());
    }
}
