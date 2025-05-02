using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class HealthBarManager : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetUp(int maxVal, Color color)
    {
        slider.maxValue = maxVal;
        slider.value = maxVal;

        fill.color = color;
        //fill.color = gradient.Evaluate(1f);
    }

    public void SetValue(int value)
    {
        slider.value = value;
        //fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}

