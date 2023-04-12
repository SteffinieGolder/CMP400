using UnityEngine;

//Behaviour script for the axe tool. 
//Used to chop trees. 

[CreateAssetMenu(menuName = "Tool Behaviour/Axe")]

public class AxeBehaviour : ToolBehaviour
{
    //Class variables.
    ItemData itemData;
    CharacterData charData;
    RaycastHit2D hit;

    //Function which checks if this tool can be used.
    //position is the screen position that was clicked by the user.
    //item is the data for the tool they want to use. 
    public override bool CheckUseConditions(Vector2 position, ItemData item)
    {
        //Data for the item.
        itemData = item;
        //Data for the current active player. 
        charData = GameManager.instance.characterManager.activePlayer.charData;

        //Raycast at screen position. 
        hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(position));

        if (hit)
        {
            //Checks if the transform hit by the mouse click is a tree. 
            if (hit.transform.gameObject.layer == 6)
            {
                //Check if player position is in the interact range of this tool. 
                if (Vector3.Distance(GameManager.instance.characterManager.activePlayer.transform.position, hit.transform.position) <= itemData.interactRange)
                {
                    //Checks if the current active player is the ADHD character.
                    if (GameManager.instance.characterManager.char1IsActive)
                    {
                        //Checks if the previous task is complete and allows player to use the tool if so. 
                        if (GameManager.instance.taskManager.isWeedingComplete)
                        {
                            return true;
                        }

                        else
                        {
                            //Checks which task stage the character is at and displays the corresponding dialogue line. 
                            if (!GameManager.instance.taskManager.isFishingComplete)
                            {
                                if (!GameManager.instance.taskManager.hasFishingStarted)
                                {
                                    GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().DisplayShouldBeFishingDialogue();
                                }

                                else
                                {
                                    GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().DisplayBusyFishingDialogue();
                                }
                            }

                            else if (!GameManager.instance.taskManager.isPlantingComplete)
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

                            /*else if (!GameManager.instance.taskManager.isWeedingComplete)
                            {
                                if (!GameManager.instance.taskManager.hasWeedingStarted)
                                {
                                    //GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().DisplayShouldBePlantingDialogue();
                                    Debug.Log("I should weed");
                                }

                                else
                                {
                                    Debug.Log("Just let me weed in peace");
                                    //GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().DisplayBusyPlantingDialogue();
                                }
                            }*/
                        }
                    }

                    //If active character is NT, then return true. 
                    else
                    {
                        return true;
                    }
                }
            }
        }

        //Return false if all conditions fail. Player cannot use this tool. 
        return false;
    }

    //Function which performs the tool behaviour. 
    public override bool PerformBehaviour()
    {
        //Set animation for player character. 
        GameManager.instance.characterManager.activePlayer.GetComponent<CharMovement>().animator.SetTrigger("chopTrigger");

        //Chop the tree.
        hit.transform.gameObject.GetComponent<TreeScript>().ChopTree();

        //Checks if ADHD character is active.
        if (GameManager.instance.characterManager.char1IsActive)
        {
            //Checks if player has complete a portion of the task (chopped one tree).
            if (GameManager.instance.taskManager.IsTaskPortionComplete(true, itemData.taskIndex))
            {
                //Update the ADHD character's energy and timer based on the tool's data. 
                GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateBehaviour(itemData.ADHDTimeValue, itemData.ADHDMultiplier, false);
            }
        }

        //NT character is active.
        else
        {
            //If NT character has completed a task portion (chopped on tree).
            if (GameManager.instance.taskManager.IsTaskPortionComplete(false, itemData.taskIndex))
            {
                //Update NT characters energy and timer values based on tool data.
                GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateBehaviour(itemData.NTTimeValue, itemData.NTMultiplier, false);
            }
        }

        //Return false to indicate that item is reusable and should not be removed from inventory. 
        return false;
    }
}
