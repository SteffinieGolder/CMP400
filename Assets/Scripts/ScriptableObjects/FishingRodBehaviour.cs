using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tool Behaviour/Fishing Rod")]

public class FishingRodBehaviour : ToolBehaviour
{
    TileManager manager;
    TileData tileData;
    Vector3Int gridPos;
    ItemData itemData;
    public override bool CheckUseConditions(Vector2 position, ItemData item)
    {
        manager = GameManager.instance.tileManager;
        gridPos = manager.GetGridPosition(position, true, TileManager.tilemapOptions.GROUND);
        itemData = item;

        TileBase tile = manager.GetTileBase(gridPos, TileManager.tilemapOptions.GROUND);

        //Check if the tile is in range of the fishing rod. 

        if (tile)
        {
            tileData = manager.GetTileData(tile);

            if (tileData)
            {
                if (tileData.canFish)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public override bool PerformBehaviour()
    {
        //Do any animations
        //Put fish in inventory
        //Change tile
        manager.ChangeTile(gridPos, itemData.tileToChangeTo, TileManager.tilemapOptions.GROUND);

        //Return false if item is reusable
        return false;
    }
}