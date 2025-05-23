using UnityEngine;

public class CombatEntranceTrigger : MonoBehaviour
{
    public CombatEncounterBehavior encounter;
    public bool selfDestruct = true;
    public bool tutorial = false;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) 
        {
            Debug.Log("Entering combat!");
            if (selfDestruct) { Destroy(gameObject); }
            GameManagerBehavior.enterCombat(encounter, tutorial);
        }
    }
}
