using UnityEngine;

[CreateAssetMenu(menuName = "Tool Behaviour/Axe")]

public class AxeBehaviour : ToolBehaviour
{
    ItemData itemData;
    RaycastHit2D hit;

    public override bool CheckUseConditions(Vector2 position, ItemData item)
    {
        itemData = item;
        //Raycast at mousepos and store the hit? if its a tree then condition is met.
        //RAYCAST ON THE LAYER INSTEAD?
        hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(position));
        if (hit)
        {
            if (hit.transform.gameObject.layer == 6)
            {
                return true;
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
