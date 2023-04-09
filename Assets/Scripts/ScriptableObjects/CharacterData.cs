using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Data")]
public class CharacterData : ScriptableObject
{
    [System.Serializable]
    public class DialogueGroup
    {
        public List<string> dialogueLines;
        public List<FaceType> expressionTypes;
    }

    public List<Sprite> charFaceSprites;

    [Header("Dialogue")]
    public List<DialogueGroup> dialogueGroups = new List<DialogueGroup>();

    [Header("Reject Dialogue")]
    public List<DialogueGroup> rejectDialogueGroups = new List<DialogueGroup>();

    [Header("Reject Dialogue")]
    public List<DialogueGroup> emoteDialogueGroups = new List<DialogueGroup>();


    public enum FaceType
    {
        NEUTRAL = 0,
        HAPPY = 1,
        ANGRY = 2,
        SAD = 3,
        SHOCK = 4
    }

    public DialogueGroup GetDialogueGroup(int groupIndex)
    {
        return dialogueGroups[groupIndex];
    }

    public void DisplayCharRejectDialogue(int groupIndex)
    {
        //Show Reject Dialogue lines.
        GameManager.instance.uiManager.SetDialogueData(rejectDialogueGroups[groupIndex].dialogueLines, rejectDialogueGroups[groupIndex].expressionTypes);
    }

    public void DisplayCharEmoteDialogue(int groupIndex)
    {
        //Show Reject Dialogue lines.
        GameManager.instance.uiManager.SetDialogueData(emoteDialogueGroups[groupIndex].dialogueLines, emoteDialogueGroups[groupIndex].expressionTypes);
    }
}
