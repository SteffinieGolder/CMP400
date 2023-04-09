using UnityEngine;

[CreateAssetMenu(menuName = "Emote Data")]

public class EmoteData : ScriptableObject
{
    public string emoteName;
    public Sprite emoteSprite;
    public float moveSpeed;
    public bool doesCharAcceptTask;
    public int dialogueIndex;
    public float lowerLimit;
    public float upperLimit;
}
