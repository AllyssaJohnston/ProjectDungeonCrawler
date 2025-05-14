using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public GameObject tooltipPanel;
    public TMP_Text tooltipText;

    public static TooltipManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        Debug.Log("tooltip manager initialized");
        tooltipPanel.SetActive(false);
    }

    public void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        tooltipPanel.SetActive(false);
    }

    void Update()
{
    if (tooltipPanel.activeSelf)
    {
        Vector3 offset = new Vector3(120f, -15f);
        tooltipPanel.transform.position = Input.mousePosition + offset;
    }
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
