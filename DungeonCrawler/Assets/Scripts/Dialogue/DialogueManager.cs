using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;

    private string[] currentLines;
    private string summaryLine;
    private int currentIndex = 0;

    private bool isActive = false;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public bool IsDialogueActive()
    {
        return isActive;
    }

    public void StartDialogue(NPC npc)
    {
        currentLines = npc.dialogueData.dialogueLines;
        currentIndex = 0;
        dialogueBox.SetActive(true);
        isActive = true;
        dialogueText.text = currentLines[currentIndex];
    }

    public void NextLine(NPC npc)
    {
        currentIndex++;
        if (currentIndex < currentLines.Length)
        {
            dialogueText.text = currentLines[currentIndex];
        }
        else
        {
            npc.finishedDialogue = true;
            EndDialogue();
        }
    }

    public void ShowSummary(NPC npc)
    {
        dialogueText.text = npc.dialogueData.summaryLine;
        dialogueBox.SetActive(true);
        isActive = true;
        currentIndex = 0;
    }



    public void EndDialogue()
    {
        dialogueBox.SetActive(false);
        isActive = false;
    }
}
