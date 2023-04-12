using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

//Scriptable object defining data for tiles.

[CreateAssetMenu(menuName = "Data/Tile Data")]
public class TileData : ScriptableObject
{
    //List of tiles associated with this tile type.
    public List<TileBase> tiles;

    //Activate any of these bool values which apply to the tiles in the list. 
    //E.g. if isPlowable is true, then all tiles in the list will be plowable and the hoe can be used on these tiles. 
    public bool isPlowable;
    public bool isPlantable;
    public bool canFish;
    public bool canBeSliced;
    public bool isHarvestable;
    public bool isWaterable;
}
