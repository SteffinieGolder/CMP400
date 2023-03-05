using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tool Behaviour/Seed")]
public class SeedBehaviour : ToolBehaviour
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
        TileBase tileCheck = manager.GetTileBase(gridPos, TileManager.tilemapOptions.GROUND);

        if (!tileCheck)
        {
            if (tile)
            {
                tileData = manager.GetTileData(tile);

                if (tileData)
                {
                    if (tileData.isPlantable)
                    {
                        if (Vector3.Distance(GameManager.instance.characterManager.activePlayer.transform.position, manager.GetWorldPosition(gridPos, TileManager.tilemapOptions.GROUND)) <= itemData.interactRange)
                        {
                            return true;
                        }
                    }
                }
            }
        }
            return false;
    }

    public override bool PerformBehaviour()
    {
        //Apply tile effect (planted seed tile)
        manager.ChangeTile(gridPos, itemData.tileToChangeTo, TileManager.tilemapOptions.GROUND);

        //return true if item needs removed 
        return true;
    }
}
