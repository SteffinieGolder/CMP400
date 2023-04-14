using UnityEngine;
using UnityEngine.Tilemaps;

//Behaviour script for the Hoe tool.
//Used to plow soil and prepare for seeds. 

[CreateAssetMenu(menuName = "Tool Behaviour/Hoe")]

public class HoeBehaviour : ToolBehaviour
{
    //Class variables.
    TileManager manager;
    TileData tileData;
    Vector3Int gridPos;
    Vector3Int gridPos2;
    ItemData itemData;
    CharacterData charData;

    //Function which checks if this tool can be used.
    //position is the screen position that was clicked by the user.
    //item is the data for the tool they want to use.
    public override bool CheckUseConditions(Vector2 position, ItemData item)
    {
        //Data for this item. 
        itemData = item;
        //Reference to the tile manager class which stores the interactable tiles in the tilemap.
        manager = GameManager.instance.tileManager;
        //Get the grid position in the background tile map for the clicked screen position.
        gridPos = manager.GetGridPosition(position, true, TileManager.tilemapOptions.BACKGROUND);
        //Get the grid position in the ground tile map for the clicked screen position.
        gridPos2 = manager.GetGridPosition(position, true, TileManager.tilemapOptions.GROUND);
        charData = GameManager.instance.characterManager.activePlayer.charData;


        //Store the tile found at the clicked position in both tilemaps. 
        TileBase tile = manager.GetTileBase(gridPos, TileManager.tilemapOptions.BACKGROUND);
        TileBase tile2 = manager.GetTileBase(gridPos2, TileManager.tilemapOptions.GROUND);

        //Checks if there's no tile on the ground layer. If there's a tile here it means a seed has been planted so the hoe
        //should not be usable. 
        //The hoe tile is in the background so seeds can be placed on top. 
        if (!tile2)
        {
            if (tile)
            {
                tileData = manager.GetTileData(tile);

                if (tileData)
                {
                    //Checks if found tile is 'plowable'. This means the hoe can be used on this tile. 
                    if (tileData.isPlowable)
                    {
                        //Checks if the player is in range to use the tool on the tile. 
                        if (Vector3.Distance(GameManager.instance.characterManager.activePlayer.transform.position, manager.GetWorldPosition(gridPos, TileManager.tilemapOptions.GROUND)) <= itemData.interactRange)
                        {
                            if (!GameManager.instance.taskManager.isSoilPrepComplete)
                            {
                                return true;
                            }

                            //Display dialogue to say that they have finished preparing the soil.
                            else
                            {
                                charData.DisplayFinishedPlantingDialogue(0);
                            }
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
        //Set the animation for the hoe behaviour. 
        GameManager.instance.characterManager.activePlayer.GetComponent<CharMovement>().animator.SetTrigger("hoeTrigger");
        //Change the tile at the clicked location to show the prepared soil. 
        manager.ChangeTile(gridPos, itemData.tileToChangeTo, TileManager.tilemapOptions.BACKGROUND);

        //Checks which character is active and updates their energy and timer values. 
        if (GameManager.instance.characterManager.char1IsActive)
        {
            GameManager.instance.taskManager.hoeTaskCounter++;
            GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateBehaviour(itemData.ADHDTimeValue, itemData.ADHDMultiplier, false);

        }

        else
        {
            GameManager.instance.taskManager.hoeTaskCounter++;
            GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateBehaviour(itemData.NTTimeValue, itemData.NTMultiplier, false);
            
        }

        //If the character has used this tool the max amount of times, set this bool to true to signal that they should not be able to use it anymore.
        if (GameManager.instance.taskManager.hoeTaskCounter == itemData.totalTaskCount)
        {
            GameManager.instance.taskManager.isSoilPrepComplete = true;
        }

        //Return false as item is a tool and is reusable. Should not be removed from the inventory. 
        return false;
    }
}
