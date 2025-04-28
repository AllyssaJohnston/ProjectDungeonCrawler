using UnityEngine.UI;
using UnityEngine;

public class CharacterIconBehavior : MonoBehaviour
{
    [SerializeField] public GameObject character;
    private Sprite characterSr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterSr = character.GetComponent<SpriteRenderer>().sprite;
        GetComponentInChildren<Image>().sprite = characterSr;
    }

    private void Update()
    {
        if (character.GetComponent<CharacterBehavior>().canCast())
        {
            //character is available
            GetComponentInChildren<Image>().color = Color.white;
        }
        else
        {
            GetComponentInChildren<Image>().color = Color.red;
        }
    }
}
