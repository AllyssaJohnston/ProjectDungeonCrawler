using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;

    private string[] currentLines;
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

    public void StartDialogue(DialogueData dialogueData)
    {
        currentLines = dialogueData.dialogueLines;
        currentIndex = 0;
        dialogueBox.SetActive(true);
        isActive = true;
        ShowLine();
    }

    public void ShowLine()
    {
        if (currentIndex < currentLines.Length)
        {
            dialogueText.text = currentLines[currentIndex];
        }
        else
        {
            EndDialogue();
        }
    }

    public void NextLine()
    {
        currentIndex++;
        ShowLine();
    }

    public void EndDialogue()
    {
        dialogueBox.SetActive(false);
        isActive = false;
    }
    
}
