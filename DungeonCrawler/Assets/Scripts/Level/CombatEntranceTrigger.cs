using UnityEngine;

public class CombatEntranceTrigger : MonoBehaviour
{
    public CombatEncounterBehavior encounter;
    public bool selfDestruct = true;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Entering combat!");
            if (selfDestruct) Destroy(this.gameObject);
            GameManagerBehavior.enterCombat(encounter);
        }
    }
}
