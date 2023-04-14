using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//Scriptable object which defines data for items.

[CreateAssetMenu(menuName = "Data/Item Data")]
public class ItemData : ScriptableObject
{
    //Item name.
    public string itemName = "Item Name";
    //Item icon.
    public Sprite icon;
    //If this item can be equipped (is it a tool?).
    public bool isEquippable;
    //The behaviour script for this item.
    public ToolBehaviour toolBehaviourScript;
    //The range at which the player can interact with this item.
    public float interactRange;
    //The tile to change to if this item is used. 
    public Tile tileToChangeTo;
    //The item to spawn if this item is used.
    public GameObject itemToSpawn;
    //The index for this item in the task list. 
    public int taskIndex;
    //Total times this tool should be used before the task is complete.
    public int totalTaskCount;

    //Timer values, multipliers for energy decreasing and the item grading image for the ADHD character when they use this item. 
    public float ADHDTimeValue;
    public float ADHDCompleteTimeValue;
    public float ADHDMultiplier;
    public Sprite ADHDGradeImage;

    //Timer values, multipliers for energy decreasing and the item grading image for the NT character when they use this item. 
    public float NTTimeValue;
    public float NTCompleteTimeValue;
    public float NTMultiplier;
    public Sprite NTGradeImage;

    //Indexes for each character for dialogue associated with this item. 
    public List<int> ADHDDialogueGroupIndexes;
    public List<int> NTDialogueGroupIndexes;

    public int itemFoundIndex;

    //Tile positions associated with this item. (If item needs to effect a patch of tiles rather than 1).
    public List<Vector2> TilePatchPositions;
    //Tile to change patch to. 
    public Tile patchTile;
    //Index used to keep track of task stages. 
    public int currentIndex;
}
