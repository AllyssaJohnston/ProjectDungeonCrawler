using UnityEngine;
using UnityEngine.UI;

public class HelpManagerBehavior : MonoBehaviour
{
    private static HelpManagerBehavior instance;

    [SerializeField] Button helpButton;
    [SerializeField] GameObject helpPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        instance.helpPanel.SetActive(false);
        instance.helpButton.gameObject.SetActive(false);
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }


    public static void showButton(bool show)
    {
        instance.helpButton.gameObject.SetActive(show);
    }

    public static void showPanel()
    {
        instance.helpPanel.SetActive(true);
        instance.helpButton.gameObject.SetActive(false);
    }

    public static void hidePanel()
    {
        instance.helpPanel.SetActive(false);
        instance.helpButton.gameObject.SetActive(true);
    }
}
