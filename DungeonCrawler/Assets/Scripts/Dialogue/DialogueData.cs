using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/NPCDialogue")]
public class DialogueData : ScriptableObject
{
    [TextArea(2, 5)]
    public string[] dialogueLines;
    public string summaryLine;
}
