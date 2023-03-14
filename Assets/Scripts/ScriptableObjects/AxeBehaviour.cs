using UnityEngine;

[CreateAssetMenu(menuName = "Tool Behaviour/Axe")]

public class AxeBehaviour : ToolBehaviour
{
    ItemData itemData;
    CharacterData charData;
    RaycastHit2D hit;

    public override bool CheckUseConditions(Vector2 position, ItemData item)
    {
        //DO A CHECK IN HERE TO SEE IF CHARACTER IS ACTUALLY GOING TO PERFORM BEHAVIOUR, IF NOT JUST ACCESS DIALOGUE/EMOTE AND DISPLAY. 
        itemData = item;
        charData = GameManager.instance.characterManager.activePlayer.charData;

        //RAYCAST ON THE LAYER INSTEAD?
        hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(position));
        if (hit)
        {
            if (hit.transform.gameObject.layer == 6)
            {
                //Check if player position is in the interact range
                if (Vector3.Distance(GameManager.instance.characterManager.activePlayer.transform.position, hit.transform.position) <= itemData.interactRange)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public override bool PerformBehaviour()
    {
        GameManager.instance.characterManager.activePlayer.GetComponent<CharMovement>().animator.SetTrigger("chopTrigger");

        //If you add in more items like the axe use polymorphism here 
        hit.transform.gameObject.GetComponent<TreeScript>().ChopTree();

        if (charData)
        {
            GameManager.instance.uiManager.ShowDialogueBox(charData.GetDialogueLine(itemData.itemName), charData.GetCharExpression(itemData.itemName), true);
        }

        if (GameManager.instance.characterManager.char1IsActive)
        {
            
            if (GameManager.instance.taskManager.IsTaskPortionComplete(true, itemData.taskIndex))
            {
                GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateBehaviour(itemData.timeValue);
            }
            else
            {
                //NOT COMPLETE
            }
        }

        else
        {            
            if(GameManager.instance.taskManager.IsTaskPortionComplete(false, itemData.taskIndex))
            {               
                GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateBehaviour(itemData.timeValue);
            }
            else
            {
                //NOT COMPLETE
            }
        }

        //Return false if item is reusable
        return false;
    }
}
