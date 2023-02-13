using UnityEngine;

//Scriptable object asset for in game items. 

[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data", order = 50)]
public class ItemData : ScriptableObject
{
    //Each item has a name and an icon. 
    public string itemName = "Item Name";
    public Sprite icon;
}
