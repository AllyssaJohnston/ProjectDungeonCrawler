using UnityEngine;

public class PopUpButton : MonoBehaviour
{
    public GameObject popupPanel;

    void Start() {
        
        if (popupPanel != null) {
            popupPanel.SetActive(false);
        } else {
            Debug.LogError("Popup Panel is broken");
        }
    }

    public void TogglePopup() {
        if (popupPanel != null) {
            bool isActive = popupPanel.activeSelf;
            popupPanel.SetActive(!isActive);
        }
    }
    public void ClosePopup()
    {
        if (popupPanel != null) {
            popupPanel.SetActive(false);
        }
    }
}