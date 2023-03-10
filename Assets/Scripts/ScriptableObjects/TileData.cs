using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Data/Tile Data")]
public class TileData : ScriptableObject
{
    public List<TileBase> tiles;

    public bool isPlowable;
    public bool isPlantable;
    public bool canFish;
    public bool canBeSliced;
    public bool isHarvestable;
    public bool isWaterable;
}
