using System.Collections;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public DialogueData dialogueData;
    private bool playerInRange = false;
    public bool finishedDialogue = false;

    public static NPC activeNPC = null;

    private void Update()
    {
        if (NPC.activeNPC != this) return; // Only active NPC responds

        if (!finishedDialogue && playerInRange && Input.GetKeyDown(KeyCode.E) && !DialogueManager.Instance.IsDialogueActive())
        {
            DialogueManager.Instance.StartDialogue(this);
        }
        else if (finishedDialogue && playerInRange && Input.GetKeyDown(KeyCode.E) && !DialogueManager.Instance.IsDialogueActive())
        {
            DialogueManager.Instance.ShowSummary(this);
        }

        if (DialogueManager.Instance.IsDialogueActive() && Input.GetKeyDown(KeyCode.Space))
        {
            if (!finishedDialogue)
            {
                DialogueManager.Instance.NextLine(this);
            }
            else
            {
                DialogueManager.Instance.EndDialogue();
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("playerInRange: " + playerInRange);
            NPC.activeNPC = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            DialogueManager.Instance.EndDialogue();
            Debug.Log("playerInRange: " + playerInRange);
            if (NPC.activeNPC == this)
            {
                NPC.activeNPC = null;
            }
        }
    }

}
