using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    // customizable scene to load
    public string sceneToLoad;

    // called when clicked
    public void LoadScene() {
        if (string.IsNullOrEmpty(sceneToLoad)) {
            Debug.Log("Main button can't load scene " + sceneToLoad);
            return;
        }
        SceneManager.LoadScene(sceneToLoad);
    }
}
