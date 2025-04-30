using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class TeamManaBehavior : MonoBehaviour
{
    public static TeamManaBehavior instance;
    private static int mana;
    private static int prevMana;
    private static TMP_Text manaText;
    private static Image image;
    private static Color regColor;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        manaText = GetComponentInChildren<TMP_Text>();
        image = GetComponentInChildren<Image>();
        regColor = image.color;
    }

    public static void updateManaWithoutEffect(int curMana)
    {
        prevMana = mana;
        mana += curMana;
        manaText.text = mana.ToString();
    }

    public static void updateMana(int curMana)
    {
        updateManaWithoutEffect(curMana);
        instance.StartCoroutine(instance.changeColorEffect());
    }

    private IEnumerator changeColorEffect()
    {
        if (mana - prevMana > 0)
        {
            image.color = Color.green;
        }
        else
        {
            image.color = Color.red;
        }
        yield return new WaitForSeconds(.2f);
        image.color = regColor;
    }

    public static int getMana() { return mana; }
}
