using UnityEngine;
using UnityEngine.Tilemaps;

//Script which controls the behaviour of the seed item.

[CreateAssetMenu(menuName = "Tool Behaviour/Seed")]
public class SeedBehaviour : ToolBehaviour
{
    //Class variables.
    TileManager manager;
    TileData tileData;
    Vector3Int gridPos;
    ItemData itemData;

    //Function which checks if this tool can be used.
    //position is the screen position that was clicked by the user.
    //item is the data for the tool they want to use.
    public override bool CheckUseConditions(Vector2 position, ItemData item)
    {
        //Data for this item. 
        itemData = item;
        //Reference to the tile manager class which stores the interactable tiles in the tilemap
        manager = GameManager.instance.tileManager;
        //Get the grid position in the tile map for the clicked screen position
        gridPos = manager.GetGridPosition(position, true, TileManager.tilemapOptions.BACKGROUND);

        //Store the tile found at the clicked position in both tilemaps. 
        TileBase tile = manager.GetTileBase(gridPos, TileManager.tilemapOptions.BACKGROUND);
        TileBase tileCheck = manager.GetTileBase(gridPos, TileManager.tilemapOptions.GROUND);

        //Checks if there's no tile on the ground layer. If there's a tile here it means a seed has been planted so 
        //should not be able to plant another one.
        if (!tileCheck)
        {
            if (tile)
            {
                tileData = manager.GetTileData(tile);

                if (tileData)
                {
                    //Checks if the selected tile is 'plantable'. This means a seed can be planted.
                    if (tileData.isPlantable)
                    {
                        //Checks if the player is within range of the tile to interact.
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

    //Function which performs the tool behaviour. 
    public override bool PerformBehaviour()
    {
        //Change the tile to show the planted seeds.
        manager.ChangeTile(gridPos, itemData.tileToChangeTo, TileManager.tilemapOptions.GROUND);

        //Checks which character is active and updates their energy and timer values. 
        if (GameManager.instance.characterManager.char1IsActive)
        {
            GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateBehaviour(itemData.ADHDTimeValue, itemData.ADHDMultiplier, false);

        }

        else
        {
            GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateBehaviour(itemData.NTTimeValue, itemData.NTMultiplier, false);

        }

        //Return true as this item is not reusable and should be removed from the inventory. 
        return true;
    }
}
