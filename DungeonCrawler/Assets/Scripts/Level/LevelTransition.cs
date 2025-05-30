using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public string level;
    public GameObject levelTransition;
    public bool ending = false;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Switching Level");
            if (!ending)
            {
                levelTransition.SetActive(true);
            }
            SceneManager.LoadScene(level, LoadSceneMode.Additive);
            if (!ending)
            {
                SceneManager.UnloadSceneAsync(this.gameObject.scene);
            }
        }
    }
}
