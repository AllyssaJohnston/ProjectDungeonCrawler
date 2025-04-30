using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatEntranceTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("FirstPerson")) {
            Debug.Log("Entering combat!");
            SceneManager.LoadScene("Combat");
        }
    }
}
