using UnityEngine;

[CreateAssetMenu(menuName = "Tool Behaviour/Hoe")]
public class HoeBehaviour : ToolBehaviour
{
    public override bool PerformBehaviour()
    {
        //Set char animation to holding the tool
        //Whenever player clicks to use, animate action
        //Set tile to plowed tile. 

        Debug.Log("Performing hoe behaviour");
        return true;
    }

}
