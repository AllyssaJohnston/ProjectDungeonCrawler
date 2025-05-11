using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendlySpellBehavior : SpellBehavior
{
    [Header("Spell details")]
    public List<FriendlyBehavior> castingCharacterBehaviors = new List<FriendlyBehavior>();
    public int moraleRegen = 0; // for party

    [Header("UI Elements")]
    public TMP_Text spellNameText;
    public TMP_Text damageText;
    public TMP_Text healText;
    public TMP_Text moraleDamageText;
    public TMP_Text moraleRegenText;

    public GameObject manaGroup;
    public Image manaImage;
    private static Color regManaColor;
    public TMP_Text manaText;

    public TMP_Text targetingText;

    public GameObject characterIconTemplate;

    private Color regPanelColor;
    private Image panelImage;

    [Header("UI Spacing")]
    public float singleCharacterX; //x pos of single character icons
    public float singleCharacterY; //y pos of single character icon
    public float singleCharacterScale;
    public float firstCharacterX; //x pos of first character icon
    public float firstCharacterY; // y pos of first character icon
    public float secondCharacterX; //x pos of second character icon
    public float secondCharacterY; // y pos of second character icon
    public float doubleCharacterScale;

    [HideInInspector] public bool canCast;

    private List<CharacterIconBehavior> characterIcons = new List<CharacterIconBehavior>();

    private void Start()
    {
        spellNameText.text = spellName;

        if (damage == 0f)
        {
            // remove morale from the list of attributes
            Destroy(damageText.gameObject);
            damageText = null;
        }
        else
        {
            damageText.text = damage + " DAMAGE";
        }

        if (heal == 0f)
        {
            // remove heal from the list of attributes
            Destroy(healText.gameObject);
            healText = null;
        }
        else
        {
            healText.text = heal + " PARTY HEALTH REGEN";
        }

        if (moraleDamageToSelf == 0f)
        {
            // remove morale from the list of attributes
            Destroy(moraleDamageText.gameObject);
            moraleDamageText = null;
        }
        else
        {
            moraleDamageText.text = moraleDamageToSelf + " SELF MORALE DAMAGE";
        }

        if (moraleRegen == 0f)
        {
            // remove morale from the list of attributes
            Destroy(moraleRegenText.gameObject);
            moraleRegenText = null;
        }
        else
        {
            moraleRegenText.text = moraleRegen + " PARTY MORALE REGEN";
        }

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
        else if (castingCharacterBehaviors.Count == 1)
        {
            //single character
            setUpIcon(singleCharacterX, singleCharacterY, singleCharacterScale, castingCharacterBehaviors[0]);
        }
        else if (castingCharacterBehaviors.Count == 2)
        {
            //double characters
            setUpIcon(firstCharacterX, firstCharacterY, doubleCharacterScale, castingCharacterBehaviors[0]);
            setUpIcon(secondCharacterX, secondCharacterY, doubleCharacterScale, castingCharacterBehaviors[1]);
            characterIcons[0].gameObject.transform.SetAsLastSibling(); // render the first character on top, which requires putting it at the bottom of the list
        }
        else
        {
            Debug.Log("Too many casting characters");
        }

        panelImage = gameObject.GetComponent<Image>();
        regPanelColor = panelImage.color;
        regManaColor = manaImage.color;
    }

    private void setUpIcon(float x, float y, float scale, FriendlyBehavior character)
    {
        GameObject curCharacterIcon = Instantiate(characterIconTemplate);
        curCharacterIcon.transform.SetParent(gameObject.transform);
        curCharacterIcon.transform.localScale = new Vector3(scale, scale, 1);
        curCharacterIcon.transform.localPosition = new Vector3(x, y, 0);

        CharacterIconBehavior curCharacterIconBehavior = curCharacterIcon.GetComponent<CharacterIconBehavior>();
        curCharacterIconBehavior.SetUp(character.iconSprite, character.iconUsedSprite);
        characterIcons.Add(curCharacterIconBehavior);
    }

    public void updateCanCast()
    {
        bool canCastSpell = true;
        // calculate if spell is castable
        for (int i = 0; i < characterIcons.Count; i++)
        {
            bool canCharacterCast = castingCharacterBehaviors[i].canCast();
            canCastSpell = canCastSpell && canCharacterCast;
        }
        int curMana = TeamManaBehavior.getMana();
        canCastSpell = canCastSpell && curMana >= manaCost;

        // update icons
        foreach (CharacterIconBehavior icon in characterIcons)
        {
            icon.updateImage(canCastSpell);
        }

        // update mana icon
        if (manaGroup.activeSelf)
        {
            manaText.text = curMana.ToString() + "/" + manaCost.ToString();
            canCastSpell = canCastSpell && curMana >= manaCost;
            if (curMana < manaCost)
            {
                manaText.color = Color.red;
                manaImage.color = new Color(.43f, .43f, .43f);
            }
            else if (!canCastSpell)
            {
                manaText.color = Color.white;
                manaImage.color = new Color(.43f, .43f, .43f);
            }
            else
            {
                manaText.color = Color.black;
                manaImage.color = regManaColor;
            }
        }

        // update panel color
        panelImage.color = canCastSpell ? regPanelColor : Color.gray;
        canCast = canCastSpell;
    }
}
