using UnityEngine;

//Script which controls the behaviour for the milk drink tool. 
//Milk (or coffee) increases energy of characters.

[CreateAssetMenu(menuName = "Tool Behaviour/Milk")]

public class MilkBehaviour : ToolBehaviour
{
    //Class variables.
    ItemData itemData;
    RaycastHit2D hit;
    CharacterData charData;

    //Function which checks if this tool can be used.
    //position is the screen position that was clicked by the user.
    //item is the data for the tool they want to use.
    public override bool CheckUseConditions(Vector2 position, ItemData item)
    {
        //Data for this item.
        itemData = item;

        //Raycast at the clicked screen position. 
        hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(position));

        if (hit)
        {
            //Checks if the player is at the clicked postion and the player (character) is active. 
            if (hit.transform.GetComponent<Player>())
            {
                if (hit.transform.GetComponent<Player>().isActiveAndEnabled)
                {
                    //Get the character data from the player.
                    charData = GameManager.instance.characterManager.activePlayer.charData;
                    //Allow the player to use the drink if they're energy levels are low enough. 
                    if (hit.transform.GetComponent<CharBehaviourBase>().GetEmoteAsString() == "Tired" || hit.transform.GetComponent<CharBehaviourBase>().GetEmoteAsString() == "Frustrated")
                    return true;
                }
            }
        }

        return false;
    }

    //Function which performs the tool behaviour. 
    public override bool PerformBehaviour()
    {
        //Checks if the ADHD character is active. 
        if (GameManager.instance.characterManager.char1IsActive)
        {
            //Displays the sky panel UI (ADHD character zones out after drinking).
            GameManager.instance.uiManager.DisplaySkyPanel(true);
            //Display dialogue to player. 
            GameManager.instance.uiManager.SetDialogueData(charData.GetDialogueGroup(itemData.ADHDDialogueGroupIndexes[0]).dialogueLines,
                charData.GetDialogueGroup(itemData.ADHDDialogueGroupIndexes[0]).expressionTypes);

            //Update the ADHD characters energy and timer values.
            GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateBehaviour(itemData.ADHDTimeValue, itemData.ADHDMultiplier, true);
            //Make time go faster after this event. 
            //GameManager.instance.dayAndNightManager.timeScale = 60f;
            
        }

        //NT character is active.
        else
        {
            //Increase NT energy and timer values.
            GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().FullyRestoreEnergy();
            GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().AdvanceTime(itemData.NTTimeValue);
            GameManager.instance.uiManager.SetDialogueData(charData.GetDialogueGroup(itemData.NTDialogueGroupIndexes[0]).dialogueLines,
              charData.GetDialogueGroup(itemData.NTDialogueGroupIndexes[0]).expressionTypes);
        }

        //Return true as item is not reusable and should be removed from the inventory. 
        return true;
    }

}
