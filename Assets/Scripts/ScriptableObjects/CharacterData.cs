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

    //public void InitDialogueLines()
    //{
    /* dialogueLines = new Dictionary<string, string>();
     faceTypes = new Dictionary<string, int>();

     dialogueLines.Add("Axe", axeLine);
     faceTypes.Add(axeLine, (int)axeFaceType);

     dialogueLines.Add("WateringCan", wateringLine);
     faceTypes.Add(wateringLine, (int)wateringFaceType);
    */

    // }

    /* public Sprite GetCharExpression(string itemName)
     {
         /*string dialogueLine = "";

         if (dialogueLines.ContainsKey(itemName))
         {
             dialogueLine = dialogueLines[itemName];

         }

         if (dialogueLine != "")
         {
             if (faceTypes.ContainsKey(dialogueLine))
             {
                 int spriteIndex = faceTypes[dialogueLine];

                 return charFaceSprites[spriteIndex];
             }
         }

         return charFaceSprites[(int)FaceType.HAPPY];
    }*/
}
