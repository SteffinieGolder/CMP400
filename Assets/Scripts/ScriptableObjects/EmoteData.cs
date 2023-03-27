using UnityEngine;

[CreateAssetMenu(menuName = "Emote Data")]

public class EmoteData : ScriptableObject
{
    public Sprite emoteSprite;
    public float moveSpeed;
    public bool doesCharAcceptTask;
}
