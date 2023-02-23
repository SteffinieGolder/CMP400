using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tool Behaviour/Sword")]

public class SwordBehaviour : ToolBehaviour
{
    TileManager manager;
    TileData tileData;
    Vector3Int gridPos;
    ItemData itemData;

    public override bool CheckUseConditions(Vector2 position, ItemData item)
    {
        //CHECK ITS WITHIN A CERTAIN RANGE////////////////////////////////////////
        manager = GameManager.instance.tileManager;
        gridPos = manager.GetGridPosition(position, true, TileManager.tilemapOptions.GROUND);
        itemData = item;

        TileBase tile = manager.GetTileBase(gridPos, TileManager.tilemapOptions.GROUND);

        if (tile)
        {
            tileData = manager.GetTileData(tile);

            if (tileData)
            {
                if (tileData.canBeSliced)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public override bool PerformBehaviour()
    {
        //Set char animation to holding the tool
        //Whenever player clicks to use, animate action
        //Remove tile

        manager.ChangeTile(gridPos, null, TileManager.tilemapOptions.GROUND);

        //Return false if item is reusable
        return false;
    }
}
