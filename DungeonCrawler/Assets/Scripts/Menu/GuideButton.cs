using UnityEngine;
using UnityEngine.UI; 

public class GuideButton : MonoBehaviour
{
    public GameObject guidePopupPanel;

    void Start() {
        
        if (guidePopupPanel != null) {
            guidePopupPanel.SetActive(false);
        } else {
            Debug.LogError("Guide Popup Panel is broken");
        }
    }

    public void ToggleGuidePopup() {
        if (guidePopupPanel != null) {
            bool isActive = guidePopupPanel.activeSelf;
            guidePopupPanel.SetActive(!isActive);
        }
    }
    public void CloseGuidePopup()
    {
        if (guidePopupPanel != null) {
            guidePopupPanel.SetActive(false);
        }
    }
}