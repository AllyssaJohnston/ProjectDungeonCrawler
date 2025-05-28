using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;

    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;

    private static NPC curNPC;
    private static int currentIndex = 0;

    private static bool isActive = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        curNPC = null;
        currentIndex = 0;
    }

    public static bool IsDialogueActive()
    {
        return isActive;
    }

    public static void StartDialogue(NPC npc)
    {
        curNPC = npc;
        currentIndex = 0;
        instance.dialogueBox.SetActive(true);
        isActive = true;
        instance.dialogueText.text = npc.dialogueData.dialogueLines[currentIndex];
    }

    public static void NextLine()
    {
        currentIndex++;
        if (currentIndex < curNPC.dialogueData.dialogueLines.Length)
        {
            instance.dialogueText.text = curNPC.dialogueData.dialogueLines[currentIndex];
        }
        else
        {
            curNPC.finishedDialogue = true;
            EndDialogue();
        }
    }

    public static void ShowSummary(NPC npc)
    {
        instance.dialogueText.text = npc.dialogueData.summaryLine;
        instance.dialogueBox.SetActive(true);
        isActive = true;
        currentIndex = 0;
    }



    public static void EndDialogue()
    {
        instance.dialogueBox.SetActive(false);
        isActive = false;
        if (curNPC != null && curNPC.destroyWhenFinished)
        {
            Destroy(curNPC.gameObject);
            curNPC = null;
        }
    }
}
