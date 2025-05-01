using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FriendlySpellBehavior : SpellBehavior
{
    [Header("Spell details")]
    [SerializeField] public List<GameObject> castingCharacters = new List<GameObject>();

    [Header("UI Elements")]
    [SerializeField] public TMP_Text spellNameText;
    [SerializeField] public TMP_Text damageText;
    [SerializeField] public TMP_Text moraleDamageText;
    [SerializeField] public TMP_Text manaText;
    [SerializeField] public TMP_Text targetingText;
    [SerializeField] public GameObject characterIconTemplate;

    [Header("UI Spacing")]
    public int singleCharacterX; //x pos of single character icons
    public int singleCharacterY; //y pos of single character icon
    public int firstCharacterX; //x pos of first character icon
    public int firstCharacterY; // y pos of first character icon
    public int secondCharacterX; //x pos of second character icon
    public int secondCharacterY; // y pos of second character icon
    public float scale = 1f;

    private List<CharacterIconBehavior> characterIcons = new List<CharacterIconBehavior>();

    private void Start()
    {
        spellNameText.text = spellName;
        damageText.text = damage + " DAMAGE";
        moraleDamageText.text = moraleDamage + " MORALE DAMAGE";
        manaText.text = manaCost.ToString();
        targetingText.text = "TARGET " + (damageAllEnemies? "ALL" : "SELECT");

        if (castingCharacters.Count == 0 )
        {
            Debug.Log("Invalid spell");
        }

        if (castingCharacters.Count == 1)
        {
            //single character
            setUpIcon(singleCharacterX, singleCharacterY, castingCharacters[0], 0);
        }
        else if (castingCharacters.Count == 2)
        {
            //double characters
            setUpIcon(secondCharacterX, secondCharacterY, castingCharacters[1], 0); // want second character to render first, so create it first
            setUpIcon(firstCharacterX, firstCharacterY, castingCharacters[0], 0); // inserts the first character icon back into the first slot
        }
        else
        {
            Debug.Log("Too many casting characters");
        }
    }

    private void setUpIcon(int x, int y, GameObject character, int index)
    {
        GameObject curCharacterIcon = Instantiate(characterIconTemplate);
        curCharacterIcon.transform.SetParent(gameObject.transform);
        curCharacterIcon.transform.localScale = new Vector3(scale, scale, 1);
        curCharacterIcon.transform.localPosition = new Vector3(x, y, 0);

        CharacterIconBehavior curCharacterIconBehavior = curCharacterIcon.GetComponent<CharacterIconBehavior>();
        curCharacterIconBehavior.SetUp(character.GetComponent<CharacterBehavior>().iconSprite);
        characterIcons.Insert(index, curCharacterIconBehavior);
    }

    private void Update()
    {
        for (int i = 0; i < characterIcons.Count; i++)
        {
            characterIcons[i].updateImage(castingCharacters[i].GetComponent<CharacterBehavior>().canCast());
        }
    }
}
