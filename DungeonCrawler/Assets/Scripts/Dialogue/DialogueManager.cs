using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager Instance;

    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;

    private static NPC curNPC;
    private static int currentIndex = 0;

    private static bool isActive = false;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public static bool IsDialogueActive()
    {
        return isActive;
    }

    public static void StartDialogue(NPC npc)
    {
        curNPC = npc;
        currentIndex = 0;
        Instance.dialogueBox.SetActive(true);
        isActive = true;
        Instance.dialogueText.text = npc.dialogueData.dialogueLines[currentIndex];
    }

    public static void NextLine()
    {
        currentIndex++;
        if (currentIndex < curNPC.dialogueData.dialogueLines.Length)
        {
            Instance.dialogueText.text = curNPC.dialogueData.dialogueLines[currentIndex];
        }
        else
        {
            curNPC.finishedDialogue = true;
            EndDialogue();
        }
    }

    public static void ShowSummary(NPC npc)
    {
        Instance.dialogueText.text = npc.dialogueData.summaryLine;
        Instance.dialogueBox.SetActive(true);
        isActive = true;
        currentIndex = 0;
    }



    public static void EndDialogue()
    {
        Instance.dialogueBox.SetActive(false);
        isActive = false;
        if (curNPC.destroyWhenFinished)
        {
            Destroy(curNPC.gameObject);
        }
    }
}
