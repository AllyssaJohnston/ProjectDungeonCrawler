using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public GameObject tooltipPanel;
    public TMP_Text tooltipDescription;
    public TMP_Text tooltipFlavor;
    [SerializeField] int xOffset;
    [SerializeField] int yOffset;

    private static TooltipManager instance;

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

    public void Setup(GameObject tooltipPanel, TMP_Text tooltipText)
    {
        
        

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
            Vector3 offset = new Vector3(instance.xOffset, instance.yOffset);
            tooltipPanel.transform.position = Input.mousePosition + offset;
        }
    }

    public static void ShowTooltip(string description, string flavor, Vector3 position)
    {
        instance.tooltipDescription.text = description;
        instance.tooltipFlavor.text = "*" + flavor + "*";
        instance.tooltipPanel.transform.position = position;
        instance.tooltipPanel.SetActive(true);
    }

    public static void HideTooltip()
    {
        instance.tooltipPanel.SetActive(false);
    }
}
