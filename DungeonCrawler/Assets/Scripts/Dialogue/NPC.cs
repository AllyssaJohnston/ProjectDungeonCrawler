using UnityEngine;

public class NPC : MonoBehaviour
{
    public DialogueData dialogueData;
    private bool playerInRange = false;
    public bool finishedDialogue = false;

    private void Update()
    {
        if (!finishedDialogue && playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Show Dialogue");
            DialogueManager.Instance.StartDialogue(this, dialogueData);
        }
        else if (finishedDialogue && playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Show summary");
            DialogueManager.Instance.ShowSummary(dialogueData);

        }

        if (!finishedDialogue && DialogueManager.Instance.IsDialogueActive() && Input.GetKeyDown(KeyCode.Space))
        {
            DialogueManager.Instance.NextLine();
        }
        else if (finishedDialogue && DialogueManager.Instance.IsDialogueActive() && Input.GetKeyDown(KeyCode.Space))
        {

            DialogueManager.Instance.EndDialogue();

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("playerInRange: " + playerInRange);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            DialogueManager.Instance.EndDialogue();
        }
    }
}
