using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class TeamManaBehavior : MonoBehaviour
{
    private static TeamManaBehavior instance = null;

    private static int mana = 0;
    private static int prevMana = 0;
    public TMP_Text manaText;
    public Image image;
    public TMP_Text manaRegenText;
    private static Color regColor;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        regColor = image.color;
        manaRegenText.text = "+ " + CombatManagerBehavior.getManaRegen();
        updateManaWithoutEffect(0);
        Debug.Log("team mana manager initialized");
    }

    private TeamManaBehavior() {}

    public static void setManaWithoutEffect(int curMana)
    {
        prevMana = curMana;
        mana = curMana;
        instance.manaText.text = mana.ToString();
        instance.image.color = regColor;
    }

    public static void updateManaWithoutEffect(int curMana)
    {
        prevMana = mana;
        mana += curMana;
        instance.manaText.text = mana.ToString();
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
