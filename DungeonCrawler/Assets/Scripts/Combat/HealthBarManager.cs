using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] Slider slider;
    //public Gradient gradient;
    [SerializeField] Image fill;
    [SerializeField] TMP_Text numberText;

    public void SetUp(int maxVal, Color color, Color textColor)
    {
        slider.maxValue = maxVal;
        slider.value = maxVal;

        numberText.text = maxVal.ToString();
        numberText.color = textColor;

        fill.color = color;
        //fill.color = gradient.Evaluate(1f);
    }

    public void SetValue(int value)
    {
        slider.value = value;
        //fill.color = gradient.Evaluate(slider.normalizedValue);
        numberText.text = value.ToString();
    }
}

