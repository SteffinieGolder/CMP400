using UnityEngine;

//Scriptable object which stores the data which defines emotes. 

[CreateAssetMenu(menuName = "Emote Data")]

public class EmoteData : ScriptableObject
{
    //Name of emote.
    public string emoteName;
    //Emote sprite.
    public Sprite emoteSprite;
    //Movement speed modifier.
    public float moveSpeed;
    //Will the character accept tasks when feeling this emote?
    public bool doesCharAcceptTask;
    //Index for dialogue to display when this emote is active before coffee.
    public int dialogueIndexPreCoffee;
    //Index for dialogue to display when this emote is active after coffee.
    public int dialogueIndexPostCoffee;
    //Lower energy limit for this emote.
    public float lowerLimit;
    //Upper energy limit for this emote. 
    public float upperLimit;
}
