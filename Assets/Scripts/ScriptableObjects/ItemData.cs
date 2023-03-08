using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//Scriptable object asset for in game items. 

[CreateAssetMenu(menuName = "Data/Item Data")]
public class ItemData : ScriptableObject
{
    //Each item has a name and an icon. 
    public string itemName = "Item Name";
    public Sprite icon;
    public bool isEquippable;
    public ToolBehaviour toolBehaviourScript;
    public float interactRange;
    public Tile tileToChangeTo;
    public GameObject itemToSpawn;
    public int taskIndex;
    public float timeValue;
}
