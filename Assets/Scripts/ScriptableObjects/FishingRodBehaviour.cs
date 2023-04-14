using UnityEngine;
using UnityEngine.Tilemaps;

//Behaviour script for the fishing rod tool.
//Used to catch fish. 

[CreateAssetMenu(menuName = "Tool Behaviour/Fishing Rod")]

public class FishingRodBehaviour : ToolBehaviour
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
        //Get the grid position in the tile map for the clicked screen position.
        gridPos = manager.GetGridPosition(position, true, TileManager.tilemapOptions.GROUND);
        charData = GameManager.instance.characterManager.activePlayer.charData;

        //Store the tile found at the clicked position.
        TileBase tile = manager.GetTileBase(gridPos, TileManager.tilemapOptions.GROUND);

        if (tile)
        {
            tileData = manager.GetTileData(tile);

            if (tileData)
            {
                //Check if the tile at the selected position and if it can be 'fished'.
                //This means the fishing rod can be used on this tile. 
                if (tileData.canFish)
                {
                    //Checks if the player is within range of the selected tile. 
                    if (Vector3.Distance(GameManager.instance.characterManager.activePlayer.transform.position, manager.GetWorldPosition(gridPos, TileManager.tilemapOptions.GROUND)) <= itemData.interactRange)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    //Function which performs the tool behaviour depending on active character.  
    public override bool PerformBehaviour()
    {
        //Increment counter which keeps track of how many fish this character has attempted to catch. 
        GameManager.instance.taskManager.fishTaskCounter++;
        int current = GameManager.instance.taskManager.fishTaskCounter;

        //Checks if ADHD character is active.
        if (GameManager.instance.characterManager.char1IsActive)
        {
            //Sets this bool to true to signal that the fishing task has started. 
            if (!GameManager.instance.taskManager.hasFishingStarted)
            {
                GameManager.instance.taskManager.hasFishingStarted = true;
            }

            //Checks which stage the user is at with the fishing task.
            if (current == 2 || current == 3 || current == 4 || current == 7 || current == 9 || current == 10)
            {
                //Show Dialogue lines depending on current task number. 
                GameManager.instance.uiManager.SetDialogueData(charData.GetDialogueGroup(itemData.ADHDDialogueGroupIndexes[itemData.currentIndex]).dialogueLines,
                    charData.GetDialogueGroup(itemData.ADHDDialogueGroupIndexes[itemData.currentIndex]).expressionTypes);

                //Update character energy and day timer.
                GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateBehaviour(itemData.ADHDTimeValue, itemData.ADHDMultiplier, false);

                //Alter movement speed.
                GameManager.instance.characterManager.char1PlayerScript.GetComponent<CharMovement>().moveSpeed -= 0.3f;

                //Checks if user has reached the last task.
                if (current == 10)
                {
                    //Signals that the task is complete and resets the bool which says if the task has started or not. 
                    GameManager.instance.taskManager.isFishingComplete = true;
                    GameManager.instance.taskManager.hasFishingStarted = false;
                    GameManager.instance.characterManager.char1PlayerScript.GetComponent<CharMovement>().moveSpeed = 3.5f;
                    //GameManager.instance.taskManager.AdvanceTimeForward();
                }

                //Increment the index which selects which dialogue to show. 
                itemData.currentIndex++;
                //Changes the tile at the fish bubble location once it has been processed. (Removes the fish bubble). 
                manager.ChangeTile(gridPos, itemData.tileToChangeTo, TileManager.tilemapOptions.GROUND);
            }

            //If ADHD char is active but the current index isn't one which should trigger dialogue, run the standard fishing behaviour. 
            else
            {
                RunBehaviour();
            }
        }

        //If NT character is active, check if they should miss a fish and say a dialogue line or run the normal behaviour. 
        else
        {
            if (current == 7)
            {
                //Show Dialogue lines at selected position in dialogue group. 
                GameManager.instance.uiManager.SetDialogueData(charData.GetDialogueGroup(itemData.NTDialogueGroupIndexes[0]).dialogueLines,
                    charData.GetDialogueGroup(itemData.NTDialogueGroupIndexes[0]).expressionTypes);

                //Changes the tile at the fish bubble location once it has been processed. (Removes the fish bubble). 
                manager.ChangeTile(gridPos, itemData.tileToChangeTo, TileManager.tilemapOptions.GROUND);
                GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateBehaviour(itemData.NTTimeValue, itemData.NTMultiplier, false);
            }

            else
            {
                RunBehaviour();
            }
        }

        //Return false as item is a tool and is reusable. 
        return false;
    }

    //Function which runs the standard fishing behaviour. 
    void RunBehaviour()
    {
        //Run fishing animation.
        GameManager.instance.characterManager.activePlayer.GetComponent<CharMovement>().animator.SetTrigger("fishTrigger");

        //Change the fish bubble tile to a plain water tile. 
        manager.ChangeTile(gridPos, itemData.tileToChangeTo, TileManager.tilemapOptions.GROUND);

        //Spawn a fish next to the player. 
        Instantiate(itemData.itemToSpawn, GameManager.instance.characterManager.activePlayer.gameObject.GetComponent<CharMovement>().GetItemSpawnPos(), GameManager.instance.characterManager.activePlayer.transform.rotation);

        //Checks if the current active character is ADHD.
        if (GameManager.instance.characterManager.char1IsActive)
        {
            //Updates the characters energy and timer if a task portion is complete (they caught a fish).
            if (GameManager.instance.taskManager.IsTaskPortionComplete(true, itemData.taskIndex))
            {
                GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateBehaviour(itemData.ADHDTimeValue, itemData.ADHDMultiplier, false);
            }          
        }

        //The NT character is active.
        else
        {
            //Updates the NT characters energy and timer if a task portion is complete (they caught a fish).
            if (GameManager.instance.taskManager.IsTaskPortionComplete(false, itemData.taskIndex))
            {
                GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateBehaviour(itemData.NTTimeValue, itemData.NTMultiplier, false);

                //Checks if the task is totally complete (checks off task on the list). 
                if (GameManager.instance.taskManager.IsTaskTotallyComplete(false, itemData.taskIndex))
                {

                }

                //If the character has caught the max amount of fish, reduce the total task counter and display the finished fishing dialogue. 
                else if (GameManager.instance.taskManager.fishTaskCounter > itemData.totalTaskCount)
                {
                    GameManager.instance.taskManager.totalTaskCounter--;
                    GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().DisplayBusyOrFinishedFishingDialogue();
                }
            }
        }
    }
}
