using UnityEngine;
public class CombatEntranceTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("FirstPerson")) {
            Debug.Log("Entering combat!");
            GameManagerBehavior.enterCombat();
        }
    }
}
