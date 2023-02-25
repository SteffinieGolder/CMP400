using UnityEngine;

[CreateAssetMenu(menuName = "Tool Behaviour/Axe")]

public class AxeBehaviour : ToolBehaviour
{
    ItemData itemData;
    RaycastHit2D hit;

    public override bool CheckUseConditions(Vector2 position, ItemData item)
    {
        itemData = item;

        //RAYCAST ON THE LAYER INSTEAD?
        hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(position));
        if (hit)
        {
            if (hit.transform.gameObject.layer == 6)
            {
                //Check if player position is in the interact range
                if (Vector3.Distance(GameManager.instance.player.transform.position, hit.transform.position) <= itemData.interactRange)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public override bool PerformBehaviour()
    {
        //If you add in more items like the axe use polymorphism here 
        hit.transform.gameObject.GetComponent<TreeScript>().ChopTree();
        //Return false if item is reusable
        return false;
    }
}
