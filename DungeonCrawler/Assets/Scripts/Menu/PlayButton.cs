using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    
    public void LoadScene() {
        string sceneToLoad = TrackerScript.Instance.sceneToLoad;
        bool started = TrackerScript.Instance.started;

        if (string.IsNullOrEmpty(sceneToLoad)) {
            Debug.Log("Main button can't load scene " + sceneToLoad);
            return;
        }
        
        if (!started) { 
            SceneManager.LoadScene(sceneToLoad); 
        } else { 
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));
            Time.timeScale = 1f;
            SceneManager.UnloadSceneAsync("Menu");
        }
    }
}
