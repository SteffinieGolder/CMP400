using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tool Behaviour/Watering Can")]

public class WateringCanBehaviour : ToolBehaviour
{
    TileManager manager;
    TileData tileData;
    Vector3Int gridPos;
    ItemData itemData;
    public override bool CheckUseConditions(Vector2 position, ItemData item)
    {
        manager = GameManager.instance.tileManager;
        gridPos = manager.GetGridPosition(position, true, TileManager.tilemapOptions.BACKGROUND);
        itemData = item;

        TileBase tile = manager.GetTileBase(gridPos, TileManager.tilemapOptions.BACKGROUND);
        
        if (tile)
        {
            tileData = manager.GetTileData(tile);

            if (tileData)
            {
                if (tileData.isPlantable)
                {
                    if (Vector3.Distance(GameManager.instance.player.transform.position, manager.GetWorldPosition(gridPos, TileManager.tilemapOptions.GROUND)) <= itemData.interactRange)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public override bool PerformBehaviour()
    {
        //Do any animations
        //Change tile. 
        manager.ChangeTile(gridPos, itemData.tileToChangeTo, TileManager.tilemapOptions.BACKGROUND);

        //Return false if item is reusable
        return false;
    }
}
