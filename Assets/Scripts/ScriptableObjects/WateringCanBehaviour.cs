using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

//Script which controls the behaviour for the watering can tool. 
//Tool which can water seeds once planted. 
//Final stage in planting task. 

[CreateAssetMenu(menuName = "Tool Behaviour/Watering Can")]

public class WateringCanBehaviour : ToolBehaviour
{
    //Class variables. 
    TileManager manager;
    TileData tileData;
    Vector3Int gridPos;
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
        //Get the grid position in the ground tile map for the clicked screen position.
        gridPos = manager.GetGridPosition(position, true, TileManager.tilemapOptions.GROUND);
        //Store the active character's data.
        charData = GameManager.instance.characterManager.activePlayer.charData;

        //Store the tile found at the clicked position in both tilemaps. 
        TileBase tile = manager.GetTileBase(gridPos, TileManager.tilemapOptions.GROUND);
        TileBase backgroundTile = manager.GetTileBase(gridPos, TileManager.tilemapOptions.BACKGROUND);

        //Checks if there is already a watered tile at the clicked position.
        //Ensures tile can't be watered twice. 
        if (backgroundTile)
        {
            if(backgroundTile.name == itemData.tileToChangeTo.name)
            {
                return false;
            }
        }

        if (tile)
        {
            tileData = manager.GetTileData(tile);

            if (tileData)
            {
                //Checks if the ground tile can be watered. This means the watering can should be used.
                if (tileData.isWaterable)
                {

                    //Checks if player is within range of tile to interact.
                    if (Vector3.Distance(GameManager.instance.characterManager.activePlayer.transform.position, manager.GetWorldPosition(gridPos, TileManager.tilemapOptions.GROUND)) <= itemData.interactRange)
                    {
                        //Checks if ADHD character is active.
                        if (GameManager.instance.characterManager.char1IsActive)
                        {
                            //Checks if this task is still ongoing.
                            if (!GameManager.instance.taskManager.isPlantingComplete)
                            {

                                return true;
                            }

                            //Displays dialogue if the task is complete. 
                            else
                            {
                                GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().DisplayRejectDialogue();
                            }
                        }

                        //NT char is active so return true. 
                        else
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
        //Perform animations.
        GameManager.instance.characterManager.activePlayer.GetComponent<CharMovement>().animator.SetTrigger("waterTrigger");

        //Change tile behind seed tile to the watered tile. 
        manager.ChangeTile(gridPos, itemData.tileToChangeTo, TileManager.tilemapOptions.BACKGROUND);

        //Checks if ADHD character is active.
        if (GameManager.instance.characterManager.char1IsActive)
        {
            //Checks if task portion is complete (user has watered a tile).
            if (GameManager.instance.taskManager.IsTaskPortionComplete(true, itemData.taskIndex))
            {
                //Checks if all available tiles have been watered. 
                if (GameManager.instance.taskManager.IsTaskTotallyComplete(true, itemData.taskIndex))
                {
                    //ADHD Character goes on without player and plants too many seeds.

                    //Activate Fade out UI. 
                    GameManager.instance.uiManager.FadeInOrOut(true);

                    //Change all tiles to seeds.
                    GameManager.instance.tileManager.ChangeAllPatchTiles(GameManager.instance.tileManager.GetAllPatchGridPositions(itemData.TilePatchPositions,
                        TileManager.tilemapOptions.GROUND), TileManager.tilemapOptions.GROUND, itemData.patchTile);

                    //Remove all seeds from the inventory.
                    Item seedItem1 = GameManager.instance.itemManager.GetItemByName("Carrot Seeds");

                    if (seedItem1)
                    {
                        GameManager.instance.characterManager.activePlayer.inventoryManager.backpack.RemoveAllItemsOfType(seedItem1);
                        GameManager.instance.characterManager.activePlayer.inventoryManager.toolbar.RemoveAllItemsOfType(seedItem1);
                        GameManager.instance.uiManager.RefreshAll();
                    }

                    //Add any remaining crops to the inventory.
                    Item carrot = GameManager.instance.itemManager.GetItemByName("Carrot");
                    Item strawb = GameManager.instance.itemManager.GetItemByName("Strawberry");
                    Item tomato = GameManager.instance.itemManager.GetItemByName("Tomato");

                    if (carrot && strawb && tomato)
                    {
                        int carrotsInStorage = GameManager.instance.characterManager.activePlayer.inventoryManager.storage.ReturnItemCount(carrot);
                        int strawbsInStorage = GameManager.instance.characterManager.activePlayer.inventoryManager.storage.ReturnItemCount(strawb);
                        int tomatsInStorage = GameManager.instance.characterManager.activePlayer.inventoryManager.storage.ReturnItemCount(tomato);

                        GameManager.instance.characterManager.activePlayer.inventoryManager.backpack.AddItemToAmount(20 - carrotsInStorage, carrot);
                        GameManager.instance.characterManager.activePlayer.inventoryManager.backpack.AddItemToAmount(9 - strawbsInStorage, strawb);
                        GameManager.instance.characterManager.activePlayer.inventoryManager.backpack.AddItemToAmount(11 - tomatsInStorage, tomato);
                    }

                    //Destroy any vegetables remaining in the environment. 
                    GameObject[] arr = GameObject.FindGameObjectsWithTag("Item");

                    foreach (var obj in arr)
                    {
                        ItemData itemData = obj.GetComponent<Item>().data;
                        if (itemData.itemName == "Carrot" || itemData.itemName == "Strawberry" || itemData.itemName == "Tomato")
                        {
                            Destroy(obj);
                        }
                    }

                    //Change some tiles to watered. 
                    List<Vector2> lessPositions = new List<Vector2>();
                    for (int i = 0; i < itemData.TilePatchPositions.Count / 2; i++)
                    {
                        lessPositions.Add(itemData.TilePatchPositions[i]);
                    }

                    GameManager.instance.tileManager.ChangeAllPatchTiles(GameManager.instance.tileManager.GetAllPatchGridPositions(lessPositions,
                       TileManager.tilemapOptions.BACKGROUND), TileManager.tilemapOptions.BACKGROUND, itemData.tileToChangeTo);


                    //Show Dialogue lines.
                    GameManager.instance.uiManager.SetDialogueData(charData.GetDialogueGroup(itemData.ADHDDialogueGroupIndexes[0]).dialogueLines,
                        charData.GetDialogueGroup(itemData.ADHDDialogueGroupIndexes[0]).expressionTypes);

                    //Update ADHD character timer and energy values.
                    GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateBehaviour(itemData.ADHDTimeValue, itemData.ADHDMultiplier, false);
                   
                    //Update task stages. 
                    GameManager.instance.taskManager.isPlantingComplete = true;
                    GameManager.instance.taskManager.hasPlantingStarted = false;
                }

            }
        }

        //NT character is active.
        else
        {
            //Check if task portion is complete and update NT characters energy and timer values. 
            if (GameManager.instance.taskManager.IsTaskPortionComplete(false, itemData.taskIndex))
            {
                GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateBehaviour(itemData.NTTimeValue, itemData.NTMultiplier, false);

                /*if (GameManager.instance.taskManager.IsTaskTotallyComplete(false, itemData.taskIndex))
                {

                }*/
            }
        }

        //Return false as tool is reusable and shouldn't be removed from the inventory. 
        return false;
    }
}
