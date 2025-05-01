using UnityEngine;

public class CharacterUICreatorBehavior : MonoBehaviour
{
    [SerializeField] public GameObject characterStatTemplate;
    [SerializeField] public GameObject panel;
    private CharacterBehavior characterBehavior;
    private GameObject characterStat;
    private CharacterStatBehavior characterStatBehavior;

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
    }

    // Update is called once per frame
    void Update()
    {
        characterStatBehavior.updateText(characterBehavior.getHealth(), characterBehavior.getMorale());
    }
}
