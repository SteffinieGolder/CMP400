using UnityEngine;

//Scriptable object asset for in game items. 

[CreateAssetMenu(menuName = "Data/Item Data")]
public class ItemData : ScriptableObject
{
    //Each item has a name and an icon. 
    public string itemName = "Item Name";
    public Sprite icon;
    public ToolBehaviour toolBehaviourScript;
}
