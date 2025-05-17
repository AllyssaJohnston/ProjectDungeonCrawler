using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public string level;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Switching Level");
            SceneManager.LoadScene(level, LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync(this.gameObject.scene);
        }
    }
}
