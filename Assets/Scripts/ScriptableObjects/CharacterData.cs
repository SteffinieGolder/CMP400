using System.Collections.Generic;
using UnityEngine;

//Script which stores dialogue data for characters. 

[CreateAssetMenu(menuName = "Character Data")]
public class CharacterData : ScriptableObject
{
    [System.Serializable]

    //Dialogue group class which holds dialogue lines and facial expressions. 
    public class DialogueGroup
    {
        public List<string> dialogueLines;
        public List<FaceType> expressionTypes;
    }

    //Sprites for character expressions. 
    public List<Sprite> charFaceSprites;

    //Dialogue groups exposed in editor.
    [Header("Dialogue")]
    public List<DialogueGroup> dialogueGroups = new List<DialogueGroup>();

    [Header("Reject Dialogue")]
    public List<DialogueGroup> rejectDialogueGroups = new List<DialogueGroup>();

    [Header("Emote Dialogue")]
    public List<DialogueGroup> emoteDialogueGroups = new List<DialogueGroup>();

    [Header("Fishing Dialogue")]
    public List<DialogueGroup> shouldBeFishingDialogue = new List<DialogueGroup>();
    public List<DialogueGroup> busyOrFinishedFishingDialogue = new List<DialogueGroup>();

    [Header("Planting Dialogue")]
    public List<DialogueGroup> shouldBePlantingDialogue = new List<DialogueGroup>();
    public List<DialogueGroup> busyPlantingDialogue = new List<DialogueGroup>();
    public List<DialogueGroup> finishedPlantingDialogue = new List<DialogueGroup>();

    [Header("Find Axe Dialogue")]
    public List<DialogueGroup> findAxeDialogue = new List<DialogueGroup>();

    [Header("End Sequence Dialogue")]
    public List<DialogueGroup> endSequenceSoloDialogue = new List<DialogueGroup>();

    [Header("Axe Borrow Conversation Dialogue")]
    public List<DialogueGroup> axeBorrowConversationDialogue = new List<DialogueGroup>();

    //Enum used to index expression list. 
    public enum FaceType
    {
        NEUTRAL = 0,
        HAPPY = 1,
        ANGRY = 2,
        SAD = 3,
        SHOCK = 4
    }

    //Returns the base dialogue group. 
    public DialogueGroup GetDialogueGroup(int groupIndex)
    {
        return dialogueGroups[groupIndex];
    }

    //Displays the reject dialogue when a character refuses to do a task. 
    public void DisplayCharRejectDialogue(int groupIndex)
    {
        GameManager.instance.uiManager.SetDialogueData(rejectDialogueGroups[groupIndex].dialogueLines, rejectDialogueGroups[groupIndex].expressionTypes);
    }

    //Displays the emote dialogue when the character's emote changes.
    public void DisplayCharEmoteDialogue(int groupIndex)
    {
        GameManager.instance.uiManager.SetDialogueData(emoteDialogueGroups[groupIndex].dialogueLines, emoteDialogueGroups[groupIndex].expressionTypes);
    }

    //Display this dialogue when the character wants to fish but the user tries to do something else.  
    public void DisplayShouldBeFishingDialogue(int groupIndex)
    {
        GameManager.instance.uiManager.SetDialogueData(shouldBeFishingDialogue[groupIndex].dialogueLines, shouldBeFishingDialogue[groupIndex].expressionTypes);
    }

    //Display this dialogue when the character is currently fishing but the user tries to change task. 
    public void DisplayFishingDialogue(int groupIndex)
    {
        GameManager.instance.uiManager.SetDialogueData(busyOrFinishedFishingDialogue[groupIndex].dialogueLines, busyOrFinishedFishingDialogue[groupIndex].expressionTypes);
    }

    //Display this dialogue when the character wants to find their axe.
    public void DisplayFindAxeDialogue(int groupIndex)
    {
        GameManager.instance.uiManager.SetDialogueData(findAxeDialogue[groupIndex].dialogueLines, findAxeDialogue[groupIndex].expressionTypes);
    }

    //Display this dialogue when the character wants to plant seeds but the user tries to do something else.  
    public void DisplayShouldBePlantingDialogue(int groupIndex)
    {
        GameManager.instance.uiManager.SetDialogueData(shouldBePlantingDialogue[groupIndex].dialogueLines, shouldBePlantingDialogue[groupIndex].expressionTypes);
    }

    //Display this dialogue when the character is currently planting seeds but the user tries to change task. 
    public void DisplayPlantingDialogue(int groupIndex)
    {
        GameManager.instance.uiManager.SetDialogueData(busyPlantingDialogue[groupIndex].dialogueLines, busyPlantingDialogue[groupIndex].expressionTypes);
    }

    public void DisplayFinishedPlantingDialogue(int groupIndex)
    {
        GameManager.instance.uiManager.SetDialogueData(finishedPlantingDialogue[groupIndex].dialogueLines, finishedPlantingDialogue[groupIndex].expressionTypes);
    }

    //Display the solo dialogue for the end sequence (when the ADHD character is looking for their axe).
    public void DisplayEndSeqSoloDialogue(int groupIndex)
    {
        GameManager.instance.uiManager.SetDialogueData(endSequenceSoloDialogue[groupIndex].dialogueLines, endSequenceSoloDialogue[groupIndex].expressionTypes);
    }
}
