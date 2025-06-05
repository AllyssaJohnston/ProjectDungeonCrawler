using UnityEngine;

public class NPC : MonoBehaviour
{
    public DialogueData dialogueData;
    private bool playerInRange = false;
    [HideInInspector] public bool finishedDialogue = false;
    public bool destroyWhenFinished = false;

    public static NPC activeNPC = null;

    private void Update()
    {
        if (activeNPC != this) return; // Only active NPC responds

        if (!finishedDialogue && playerInRange && Input.GetKeyDown(KeyCode.E) && !DialogueManager.IsDialogueActive())
        {
            DialogueManager.StartDialogue(this);
        }
        else if (finishedDialogue && playerInRange && Input.GetKeyDown(KeyCode.E) && !DialogueManager.IsDialogueActive())
        {
            DialogueManager.ShowSummary(this);
            return;
        }

        if (DialogueManager.IsDialogueActive() && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E)))
        {
            if (!finishedDialogue)
            {
                DialogueManager.NextLine();
            }
            else
            {
                DialogueManager.EndDialogue();
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("playerInRange: " + playerInRange);
            activeNPC = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            DialogueManager.EndDialogue();
            Debug.Log("playerInRange: " + playerInRange);
            if (activeNPC == this)
            {
                activeNPC = null;
            }
        }
    }

}
