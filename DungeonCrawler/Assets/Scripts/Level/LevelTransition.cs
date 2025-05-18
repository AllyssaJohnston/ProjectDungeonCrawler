using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public string level;
    public GameObject levelTransition;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Switching Level");
            levelTransition.SetActive(true);
            SceneManager.LoadScene(level, LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync(this.gameObject.scene);
        }
    }
}
