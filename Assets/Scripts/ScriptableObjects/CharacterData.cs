using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Data")]
public class CharacterData : ScriptableObject
{
    public List<Sprite> charFaceSprites;

    [Header("Dialogue Lines")]
    public string axeLine;
    public FaceType axeFaceType;

    Dictionary<string, string> dialogueLines;
    Dictionary<string, int> faceTypes;

    public enum FaceType
    {
        NEUTRAL = 0,
        HAPPY = 1,
        ANGRY = 2,
        SAD = 3,
        SHOCK = 4
    }

    public void InitDialogueLines()
    {
        dialogueLines = new Dictionary<string, string>();
        faceTypes = new Dictionary<string, int>();

        dialogueLines.Add("Axe", axeLine);
        faceTypes.Add(axeLine, (int)axeFaceType);
    }

    public string GetDialogueLine(string itemName)
    {
        if(dialogueLines.ContainsKey(itemName))
        {
            return dialogueLines[itemName];

        }

        return null;
    }

    public Sprite GetCharExpression(string itemName)
    {
        string dialogueLine = "";

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
    }
}
