using UnityEngine;

public class CombatEntranceTrigger : MonoBehaviour
{
    public CombatEncounterBehavior encounter;
    public bool selfDestruct = true;
    public bool tutorial = false;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (tutorial)
            {
                Debug.Log("Entering tutorial!");
                if (selfDestruct) Destroy(this.gameObject);
                GameManagerBehavior.enterCombatTutorial();
            }
            else
            {
                Debug.Log("Entering combat!");
                if (selfDestruct) Destroy(this.gameObject);
                GameManagerBehavior.enterCombat(encounter);
            }
            
                
        }
    }
}
