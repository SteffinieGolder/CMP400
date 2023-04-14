using UnityEngine;
using UnityEngine.Tilemaps;

//Script which controls the behaviour for the sword tool.
//Sword is used to chop/remove weeds.

[CreateAssetMenu(menuName = "Tool Behaviour/Sword")]

public class SwordBehaviour : ToolBehaviour
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
        //Get the grid position in the background tile map for the clicked screen position.
        gridPos = manager.GetGridPosition(position, true, TileManager.tilemapOptions.GROUND);
        //Data for the current active player. 
        charData = GameManager.instance.characterManager.activePlayer.charData;

        //Store the tile found at the clicked position in the tilemap. 
        TileBase tile = manager.GetTileBase(gridPos, TileManager.tilemapOptions.GROUND);

        if (tile)
        {
            tileData = manager.GetTileData(tile);

            if (tileData)
            {
                //Check if the tile can be 'sliced'. This means that a weed tile is at this position and this tool can be used.
                if (tileData.canBeSliced)
                {
                    //Checks if the player is within range of the tile. 
                    if (Vector3.Distance(GameManager.instance.characterManager.activePlayer.transform.position, manager.GetWorldPosition(gridPos, TileManager.tilemapOptions.GROUND)) <= itemData.interactRange)
                    {
                        //Checks if the ADHD character is active.
                        if(GameManager.instance.characterManager.char1IsActive)
                        {
                            //Checks if the previous task is complete.
                            if (GameManager.instance.taskManager.isPlantingComplete)
                            {
                                //Checks if this task has already been finished.
                                if (!GameManager.instance.taskManager.isWeedingComplete)
                                {
                                    return true;
                                }

                                //Display the appropriate dialogue for this stage in the tasks. 
                                else
                                {
                                    GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().DisplayFindAxeDialogue();
                                }
                            }

                            else
                            {
                                //Display dialogue depending on which task stage the player is at. 
                                if (!GameManager.instance.taskManager.isFishingComplete)
                                {
                                    if (!GameManager.instance.taskManager.hasFishingStarted)
                                    {
                                        GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().DisplayShouldBeFishingDialogue();
                                    }

                                    else
                                    {
                                        GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().DisplayBusyOrFinishedFishingDialogue();
                                    }
                                }

                                else
                                {
                                    if (!GameManager.instance.taskManager.hasPlantingStarted)
                                    {
                                        GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().DisplayShouldBePlantingDialogue();
                                    }

                                    else
                                    {
                                        GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().DisplayBusyPlantingDialogue();
                                    }
                                }
                            }
                        }

                        //Return true if NT character is active.
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
        //Display character animation.
        GameManager.instance.characterManager.activePlayer.GetComponent<CharMovement>().animator.SetTrigger("slashTrigger");

        //Remove the weed tile from the clicked position. 
        manager.ChangeTile(gridPos, null, TileManager.tilemapOptions.GROUND);

        //Spawn a weed item which can be picked up in the environment. 
        Instantiate(itemData.itemToSpawn, GameManager.instance.characterManager.activePlayer.gameObject.GetComponent<CharMovement>().GetItemSpawnPos(), GameManager.instance.characterManager.activePlayer.transform.rotation);

        //Checks if ADHD character is active.
        if (GameManager.instance.characterManager.char1IsActive)
        {
            //Increments task counters and updates character energy if they complete a task portion (slice one weed).
            if (GameManager.instance.taskManager.IsTaskPortionComplete(true, itemData.taskIndex))
            {
                GameManager.instance.taskManager.hasWeedingStarted = true;
                GameManager.instance.taskManager.weedTaskCounter++;
                GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateBehaviour(itemData.ADHDTimeValue, itemData.ADHDMultiplier, false);

                //Complete the task and display dialogue if this number of weeds have been sliced.
                if (GameManager.instance.taskManager.weedTaskCounter == 10)
                {
                    GameManager.instance.taskManager.isWeedingComplete = true;
                    GameManager.instance.taskManager.hasWeedingStarted = false;
                    GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().DisplayFindAxeDialogue();
                    //GameManager.instance.taskManager.AdvanceTimeForward();
                }
            }           
        }

        else
        {
            //NT character is active so update their energy bar and timer if they complete a task portion.
            if (GameManager.instance.taskManager.IsTaskPortionComplete(false, itemData.taskIndex))
            {
                GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateBehaviour(itemData.NTTimeValue, itemData.NTMultiplier, false);

                //Checks if the task is totally complete (checks off task on the list). 
                if (GameManager.instance.taskManager.IsTaskTotallyComplete(false, itemData.taskIndex))
                {
                    //Show Dialogue at desired index. This will be the NT character saying the task is finished. 
                    GameManager.instance.uiManager.SetDialogueData(charData.GetDialogueGroup(itemData.NTDialogueGroupIndexes[0]).dialogueLines,
                        charData.GetDialogueGroup(itemData.NTDialogueGroupIndexes[0]).expressionTypes);

                    GameManager.instance.taskManager.totalTaskCounter--;

                    //GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().AdvanceTime(itemData.NTCompleteTimeValue);
                }
            }
        }

        //Return false as item is tool and is reusable. This ensures this item isn't removed from the inventory. 
        return false;
    }
}
