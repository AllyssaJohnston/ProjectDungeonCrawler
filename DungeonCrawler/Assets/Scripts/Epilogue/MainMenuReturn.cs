using UnityEngine;

public class MainMenuReturn : MonoBehaviour
{
    public void returnToMainMenu()
    {
        GameObject.FindWithTag("MenuData").SetActive(true);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
