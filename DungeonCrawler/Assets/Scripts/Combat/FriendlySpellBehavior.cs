using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendlySpellBehavior : SpellBehavior
{
    [Header("Spell details")]
    public List<FriendlyBehavior> castingCharacterBehaviors = new List<FriendlyBehavior>();
    public bool stun = false;
    public int moraleDamageToSelf = 0; // to the casting character
    public int moraleRegen = 0; // for party
    public bool revive = false;
    public int manaRegen = 0; // for the party
    public int manaCost = 0;

    [Header("UI Elements")]
    public TMP_Text spellNameText;
    public Image spellTitlePanel;
    public Image damageIcon;
    public TMP_Text damageText;
    public Image stunIcon;
    public TMP_Text stunText;
    public Image reviveIcon;
    public TMP_Text reviveText;
    public Image healIcon;
    public TMP_Text healText;
    public Image moraleDamageIcon;
    public TMP_Text moraleDamageText;
    public Image moraleRegenIcon;
    public TMP_Text moraleRegenText;
    public Image manaRegenIcon;
    public TMP_Text manaRegenText;
    public GameObject targetIconSlot;
    public Sprite targetSingleIcon;
    public Sprite targetAllIcon;
    public TMP_Text targetingText;

    private List<Image> iconImages = new List<Image>();

    public GameObject manaGroup;
    public Image manaImage;
    private static Color regManaColor;
    public TMP_Text manaText;

    public GameObject characterIconTemplate;

    private Color regPanelColor;
    private Color regTitlePanelColor;
    private Color grayTitlePanelColor;
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
        panelImage = gameObject.GetComponent<Image>();
        regPanelColor = panelImage.color;
        regTitlePanelColor = new Color(regPanelColor.r, regPanelColor.g, regPanelColor.b, .75f);
        grayTitlePanelColor = new Color(Color.gray.r, Color.gray.g, Color.gray.b, .75f);
        regManaColor = manaImage.color;
        setUpTextIcons();
        setUpStringStats();
    }

    private void setUpTextIcons()
    {
        iconImages.Clear();
        spellNameText.text = spellName;

        if (damage == 0f)
        {
            // remove morale from the list of attributes
            Destroy(damageText.gameObject); damageText = null;
            Destroy(damageIcon.gameObject); damageIcon = null;

        }
        else
        {
            damageText.text = damage + " DAMAGE";
            iconImages.Add(damageIcon);
        }

        if (!stun)
        {
            // remove morale from the list of attributes
            Destroy(stunText.gameObject); stunText = null;
            Destroy(stunIcon.gameObject); stunIcon = null;

        }
        else
        {
            stunText.text = "STUN ENEMY";
            iconImages.Add(stunIcon);
        }

        if (!revive)
        {
            // remove morale from the list of attributes
            Destroy(reviveText.gameObject); reviveText = null;
            Destroy(reviveIcon.gameObject); reviveIcon = null;

        }
        else
        {
            reviveText.text = "REVIVE 1 PARTY MEMBER";
            iconImages.Add(reviveIcon);
        }

        if (heal == 0f)
        {
            // remove heal from the list of attributes
            Destroy(healText.gameObject); healText = null;
            Destroy(healIcon.gameObject); healIcon = null;
        }
        else
        {
            healText.text = heal + " PARTY HEALTH REGEN";
            iconImages.Add(healIcon);
        }

        if (moraleDamageToSelf == 0f)
        {
            // remove morale from the list of attributes
            Destroy(moraleDamageText.gameObject); moraleDamageText = null;
            Destroy(moraleDamageIcon.gameObject); moraleDamageIcon = null;
        }
        else
        {
            moraleDamageText.text = moraleDamageToSelf + " SELF MORALE DAMAGE";
            iconImages.Add(moraleDamageIcon);
        }

        if (moraleRegen == 0f)
        {
            // remove morale from the list of attributes
            Destroy(moraleRegenText.gameObject); moraleRegenText = null;
            Destroy(moraleRegenIcon.gameObject); moraleRegenIcon = null;

        }
        else
        {
            moraleRegenText.text = moraleRegen + " PARTY MORALE REGEN";
            iconImages.Add(moraleRegenIcon);
        }

        if (manaRegen == 0f)
        {
            // remove mana regen from the list of attributes
            Destroy(manaRegenText.gameObject); manaRegenText = null;
            Destroy(manaRegenIcon.gameObject); manaRegenIcon = null;
        }
        else
        {
            manaRegenText.text = manaRegen + " PARTY MANA REGEN";
            iconImages.Add(manaRegenIcon);
        }

        if (manaCost == 0)
        {
            //hide the mana group
            manaGroup.SetActive(false);
        }

        switch(targeting)
        {
            case E_SPELL_TARGETING.SINGLE_ENEMY:
                targetingText.text = "SINGLE ENEMY";
                targetIconSlot.GetComponent<Image>().sprite = targetSingleIcon;
                break;
            case E_SPELL_TARGETING.ALL_ENEMIES:
                targetingText.text = "ALL ENEMIES";
                targetIconSlot.GetComponent<Image>().sprite = targetAllIcon;
                break;
            case E_SPELL_TARGETING.SINGLE_FRIENDLY:
                targetingText.text = "SINGLE PARTY MEMBER";
                targetIconSlot.GetComponent<Image>().sprite = targetSingleIcon;
                break;
            case E_SPELL_TARGETING.ALL_FRIENDLIES:
                targetingText.text = "ALL PARTY MEMBERS";
                targetIconSlot.GetComponent<Image>().sprite = targetAllIcon;
                break;
            default:
                Debug.Log("Unrecognized spell targeting");
                break;
        }
        iconImages.Add(targetIconSlot.GetComponent<Image>());


        if (castingCharacterBehaviors.Count == 0)
        {
            Debug.Log("Invalid spell");
        }
        else if (castingCharacterBehaviors.Count == 1)
        {
            //single character
            setUpCharacterIcon(singleCharacterX, singleCharacterY, singleCharacterScale, castingCharacterBehaviors[0]);
        }
        else if (castingCharacterBehaviors.Count == 2)
        {
            //double characters
            setUpCharacterIcon(firstCharacterX, firstCharacterY, doubleCharacterScale, castingCharacterBehaviors[0]);
            setUpCharacterIcon(secondCharacterX, secondCharacterY, doubleCharacterScale, castingCharacterBehaviors[1]);
        }
        else
        {
            Debug.Log("Too many casting characters");
        }
    }

    override protected void setUpStringStats()
    {
        base.setUpStringStats();

        if (moraleDamageToSelf != 0f)
        {
            spellDescriptionText += " costing " + moraleDamageToSelf + " morale,";
        }

        if (stun)
        {
            spellDescriptionText += " stunning,";
        }

        if (revive)
        {
            spellDescriptionText += " reviving,";
        }

        if (moraleRegen != 0f)
        {
            spellDescriptionText += " for " + moraleRegen + " party morale regen,";
        }

        if (manaRegen != 0f)
        {
            spellDescriptionText += " for " + manaRegen + " party mana regen,";
        }

        if (manaCost != 0)
        {
            spellDescriptionText += " costing " + manaCost + " mana,";
        }


        if (castingCharacterBehaviors.Count == 1)
        {
            castingCharactersText = castingCharacterBehaviors[0].characterName;
        }
        else if (castingCharacterBehaviors.Count == 2)
        {
            castingCharactersText = castingCharacterBehaviors[0].characterName + " and " + castingCharacterBehaviors[1].characterName;
        }
    }

    private void setUpCharacterIcon(float x, float y, float scale, FriendlyBehavior character)
    {
        GameObject curCharacterIcon = Instantiate(characterIconTemplate);
        curCharacterIcon.transform.SetParent(gameObject.transform);
        curCharacterIcon.transform.localScale = new Vector3(scale, scale, 1);
        curCharacterIcon.transform.localPosition = new Vector3(x, y, 0);

        CharacterIconBehavior curCharacterIconBehavior = curCharacterIcon.GetComponent<CharacterIconBehavior>();
        curCharacterIconBehavior.SetUp(character.iconSprite, character.iconUsedSprite, character.iconDeadSprite);
        characterIcons.Add(curCharacterIconBehavior);
    }

    public void updateCanCast()
    {
        bool canCastSpell = true;
        // calculate if spell is castable
        bool[] canCharactersCast = new bool[characterIcons.Count];
        for (int i = 0; i < characterIcons.Count; i++)
        {
            bool canCharacterCast = castingCharacterBehaviors[i].canCast();
            canCharactersCast[i] = canCharacterCast;
            canCastSpell = canCastSpell && canCharacterCast;
        }
        int curMana = TeamManaBehavior.getMana();
        canCastSpell = canCastSpell && curMana >= manaCost;

        // update character icons
        for (int i = 0; i < characterIcons.Count; i++)
        {
            characterIcons[i].updateImage(canCastSpell, canCharactersCast[i]);
        }

        //update spell attribute icons
        foreach(Image icon in iconImages)
        {
            icon.color = canCastSpell ? Color.white : new Color(.43f, .43f, .43f);
        }

        // update mana icon
        if (manaGroup.activeSelf)
        {
            manaText.text = curMana.ToString() + "/" + manaCost.ToString();
            canCastSpell = canCastSpell && curMana >= manaCost;
            if (curMana < manaCost)
            {
                manaText.color = Color.red;
                manaImage.color = Color.red;
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
        spellTitlePanel.color = canCastSpell ? regTitlePanelColor : grayTitlePanelColor;
        canCast = canCastSpell;
    }

    public void applyDamageModifier(float damageModifer)
    {
        //TODO don't do this
        int baseDamage = damage;
        damage = (int)((float)damage * damageModifer);
        setUpStringStats();
        damage = baseDamage;
    }
}
