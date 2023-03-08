using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tool Behaviour/Bag")]

public class BagBehaviour : ToolBehaviour
{
    TileManager manager;
    TileData tileData;
    Vector3Int gridPos;
    ItemData itemData;
    TileBase tile;

    public override bool CheckUseConditions(Vector2 position, ItemData item)
    {
        manager = GameManager.instance.tileManager;
        gridPos = manager.GetGridPosition(position, true, TileManager.tilemapOptions.GROUND);
        itemData = item;

        tile = manager.GetTileBase(gridPos, TileManager.tilemapOptions.GROUND);

        if (tile)
        {
            tileData = manager.GetTileData(tile);

            if (tileData)
            {
                if (tileData.isHarvestable)
                {
                    if (Vector3.Distance(GameManager.instance.characterManager.activePlayer.transform.position, manager.GetWorldPosition(gridPos, TileManager.tilemapOptions.GROUND)) <= itemData.interactRange)
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
        Instantiate(GameManager.instance.itemManager.GetItemByName(tile.name), manager.GetWorldPosition(gridPos, TileManager.tilemapOptions.GROUND), GameManager.instance.characterManager.activePlayer.transform.rotation);
        manager.ChangeTile(gridPos, null, TileManager.tilemapOptions.GROUND);
        return false;
    }
}