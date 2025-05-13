using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipText; // Or TMP_Text if using TextMeshPro

    public static TooltipManager Instance;

    private void Awake()
    {
        Instance = this;
        tooltipPanel.SetActive(false);
    }

    public void ShowTooltip(string message, Vector3 position)
    {
        tooltipText.text = message;
        tooltipPanel.transform.position = position;
        tooltipPanel.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}
