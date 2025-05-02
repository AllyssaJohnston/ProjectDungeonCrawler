using System.Collections.Generic;
using TMPro;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.UI;

public class FriendlySpellBehavior : SpellBehavior
{
    [Header("Spell details")]
    [SerializeField] public List<CharacterBehavior> castingCharacterBehaviors = new List<CharacterBehavior>();

    [Header("UI Elements")]
    [SerializeField] public TMP_Text spellNameText;
    [SerializeField] public TMP_Text damageText;
    [SerializeField] public TMP_Text moraleDamageText;
    [SerializeField] public GameObject manaGroup;
    [SerializeField] public Image manaImage;
    private static Color regManaColor;
    [SerializeField] public TMP_Text manaText;
    [SerializeField] public TMP_Text targetingText;
    [SerializeField] public GameObject characterIconTemplate;
    private Color regPanelColor;
    private Image panelImage;

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
        if (manaCost == 0)
        {
            //hide the mana group
            manaGroup.SetActive(false);
        }
        targetingText.text = (damageAllEnemies? "TARGET ALL" : "SINGLE TARGET");

        if (castingCharacterBehaviors.Count == 0 )
        {
            Debug.Log("Invalid spell");
        }

        if (castingCharacterBehaviors.Count == 1)
        {
            //single character
            setUpIcon(singleCharacterX, singleCharacterY, castingCharacterBehaviors[0], 0);
        }
        else if (castingCharacterBehaviors.Count == 2)
        {
            //double characters
            setUpIcon(secondCharacterX, secondCharacterY, castingCharacterBehaviors[1], 0); // want second character to render first, so create it first
            setUpIcon(firstCharacterX, firstCharacterY, castingCharacterBehaviors[0], 0); // inserts the first character icon back into the first slot
        }
        else
        {
            Debug.Log("Too many casting characters");
        }

        panelImage = gameObject.GetComponent<Image>();
        regPanelColor = panelImage.color;
        regManaColor = manaImage.color;
}

    private void setUpIcon(int x, int y, CharacterBehavior character, int index)
    {
        GameObject curCharacterIcon = Instantiate(characterIconTemplate);
        curCharacterIcon.transform.SetParent(gameObject.transform);
        curCharacterIcon.transform.localScale = new Vector3(scale, scale, 1);
        curCharacterIcon.transform.localPosition = new Vector3(x, y, 0);

        CharacterIconBehavior curCharacterIconBehavior = curCharacterIcon.GetComponent<CharacterIconBehavior>();
        curCharacterIconBehavior.SetUp(character.iconSprite);
        characterIcons.Insert(index, curCharacterIconBehavior);
    }

    private void Update()
    {
        bool canCastSpell = true;
        //update icons
        for (int i = 0; i < characterIcons.Count; i++)
        {
            bool canCharacterCast = castingCharacterBehaviors[i].canCast();
            characterIcons[i].updateImage(canCharacterCast);
            canCastSpell = canCastSpell && canCharacterCast;
        }

        //update mana
        if (manaGroup.activeSelf)
        {
            int curMana = TeamManaBehavior.getMana();
            manaText.text = curMana.ToString() + "/" + manaCost.ToString();
            canCastSpell = canCastSpell && curMana >= manaCost;
            if (curMana < manaCost)
            {
                manaText.color = Color.red;
                manaImage.color = new Color(.43f, .43f, .43f);
            }
            else
            {
                manaText.color = Color.black;
                manaImage.color = regManaColor;
            }
        }

        if (canCastSpell)
        {
            panelImage.color = regPanelColor;
        }
        else
        {
            panelImage.color = Color.gray;
        }
    }
}
