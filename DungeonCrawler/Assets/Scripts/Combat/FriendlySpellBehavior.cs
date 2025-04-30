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
    [SerializeField] public GameObject panel;
    [SerializeField] public GameObject characterIconTemplate;
    [SerializeField] public int posX = 375;
    [SerializeField] public int spacing = 90;
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

        for (int i = 0; i < castingCharacters.Count; i++)
        {
            GameObject curCharacterIcon = Instantiate(characterIconTemplate);
            Vector3 scale2 = curCharacterIcon.transform.localScale * 1.2f;
            curCharacterIcon.transform.SetParent(panel.transform);
            curCharacterIcon.transform.localScale = new Vector3(scale2.x, scale2.y, 1);
            curCharacterIcon.transform.localPosition = new Vector3(posX + (i * spacing), 0, 0);
            CharacterIconBehavior curCharacterIconBehavior = curCharacterIcon.GetComponent<CharacterIconBehavior>();
            curCharacterIconBehavior.SetUp(castingCharacters[i].GetComponent<CharacterBehavior>().iconSprite);
            characterIcons.Add(curCharacterIconBehavior);
        }
    }

    private void Update()
    {
        for (int i = 0; i < characterIcons.Count; i++)
        {
            characterIcons[i].updateImage(castingCharacters[i].GetComponent<CharacterBehavior>().canCast());
        }
    }
}
