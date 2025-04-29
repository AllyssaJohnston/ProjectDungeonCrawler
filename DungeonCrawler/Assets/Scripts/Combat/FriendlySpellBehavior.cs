using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class FriendlySpellBehavior : SpellBehavior
{
    [SerializeField] public List<GameObject> castingCharacters = new List<GameObject>(); //up to 2

    [SerializeField] public TMP_Text spellNameText;
    [SerializeField] public TMP_Text damageText;
    [SerializeField] public TMP_Text moraleDamageText;
    [SerializeField] public TMP_Text manaText;
    [SerializeField] public TMP_Text targetingText;
    [SerializeField] public GameObject panel;
    [SerializeField] public GameObject characterIconTemplate;


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

        float posX = 375;
        float spacing = 90;

        for (int i = 0; i < castingCharacters.Count; i++)
        {
            GameObject curCharacterIcon = Instantiate(characterIconTemplate);
            Vector3 scale2 = curCharacterIcon.transform.localScale;
            curCharacterIcon.transform.SetParent(panel.transform);
            curCharacterIcon.transform.localScale = new Vector3(scale2.x, scale2.y, 1);
            curCharacterIcon.transform.localPosition = new Vector3(posX + (i * spacing), 0, 0);
            CharacterIconBehavior curCharacterIconBehavior = curCharacterIcon.GetComponent<CharacterIconBehavior>();
            curCharacterIconBehavior.SetUp(castingCharacters[i].GetComponent<SpriteRenderer>().sprite);
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
