using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public static GameObject tooltipPanel;
    public static TMP_Text tooltipDescription;
    public static TMP_Text tooltipFlavor;
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
        tooltipPanel.SetActive(false);
        DontDestroyOnLoad(this.gameObject);
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
        Debug.Log("tooltip manager initialized");
        
    }

    void Update()
    {
        if (tooltipPanel.activeSelf)
        {
            Vector3 offset = new Vector3(instance.xOffset, instance.yOffset);
            tooltipPanel.transform.position = Input.mousePosition + offset;
        }
    }

    public static void onLevelChange()
    {
        tooltipPanel = GameObject.FindGameObjectsWithTag("tooltipPanel")[0];
        TMP_Text[] text = tooltipPanel.GetComponentsInChildren<TMP_Text>();
        tooltipDescription = text[0];
        tooltipFlavor = text[1];
        tooltipPanel.SetActive(false);
    }

    public static void ShowTooltip(string description, string flavor, Vector3 position)
    {
        Debug.Log("Show");
        tooltipDescription.text = description;
        tooltipFlavor.text = "*" + flavor + "*";
        tooltipPanel.transform.position = position;
        tooltipPanel.SetActive(true);
    }

    public static void HideTooltip()
    {
        Debug.Log("hide");
        tooltipPanel.SetActive(false);
    }
}
