using UnityEngine;

[CreateAssetMenu(menuName = "Tool Behaviour/Milk")]

public class MilkBehaviour : ToolBehaviour
{
    ItemData itemData;
    RaycastHit2D hit;

    public override bool CheckUseConditions(Vector2 position, ItemData item)
    {
        itemData = item;

        hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(position));

        if (hit)
        {
            if (hit.transform.GetComponent<Player>())
            {
                if (hit.transform.GetComponent<Player>().isActiveAndEnabled)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public override bool PerformBehaviour()
    {
        if (GameManager.instance.characterManager.char1IsActive)
        {
            //Increase adhd energy by amount
            GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateBehaviour(itemData.ADHDTimeValue, itemData.ADHDMultiplier, true);
        }

        else
        {
            //increase nt eneergy by amount
            GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateBehaviour(itemData.NTTimeValue, itemData.NTMultiplier, true);
        }

        //Return false if item is reusable
        return true;
    }

}
