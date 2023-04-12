using UnityEngine;
using UnityEngine.Tilemaps;

//Behaviour script for the Bag tool. Only used by ADHD character.
//Used to harvest crops. 

[CreateAssetMenu(menuName = "Tool Behaviour/Bag")]

public class BagBehaviour : ToolBehaviour
{
    //Class variables. 
    TileManager manager;
    TileData tileData;
    Vector3Int gridPos;
    ItemData itemData;
    TileBase tile;
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
        //Get the grid position in the tile map for the clicked screen position.
        gridPos = manager.GetGridPosition(position, true, TileManager.tilemapOptions.GROUND);

        //Store the tile found at the clicked position.
        tile = manager.GetTileBase(gridPos, TileManager.tilemapOptions.GROUND);

        if (tile)
        {
            tileData = manager.GetTileData(tile);

            if (tileData)
            {
                //Check if the tile at the selected position is 'harvestable'.
                //A harvestable tile means a crop is at this screen position.
                if (tileData.isHarvestable)
                {
                    //Checks if the player is within range of the selected tile. 
                    if (Vector3.Distance(GameManager.instance.characterManager.activePlayer.transform.position, manager.GetWorldPosition(gridPos, TileManager.tilemapOptions.GROUND)) <= itemData.interactRange)
                    {
                        //Checks if the previous task is complete.
                        if (GameManager.instance.taskManager.isFishingComplete)
                        {
                            return true;
                        }

                        else
                        {
                            //Checks current task stage and outputs dialogue accordingly. 
                            if (!GameManager.instance.taskManager.hasFishingStarted)
                            {
                                GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().DisplayShouldBeFishingDialogue();
                            }

                            else
                            {
                                GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().DisplayBusyFishingDialogue();
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
        //Instantiate the required crop. If the current tile is a carrot in the ground, this will spawn a carrot in the environment that the player
        //can pick up. 
        Instantiate(GameManager.instance.itemManager.GetItemByName(tile.name), GameManager.instance.characterManager.activePlayer.gameObject.GetComponent<CharMovement>().GetItemSpawnPos(), 
            GameManager.instance.characterManager.activePlayer.transform.rotation);

        //Removes the tile from that position, so the layer beneath is exposed. 
        manager.ChangeTile(gridPos, null, TileManager.tilemapOptions.GROUND);

        //Update ADHD character energy bar and timer based on tool data. 
        GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateBehaviour(itemData.ADHDTimeValue, itemData.ADHDMultiplier, false);

        //Set bool to true to signal that the planting task has begun.
        if (!GameManager.instance.taskManager.hasPlantingStarted)
        {
            GameManager.instance.taskManager.hasPlantingStarted = true;
        }

        //Return false as tool is reusable and shouldn't be removed from inventory on use. 
        return false;
    }
}