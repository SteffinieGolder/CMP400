using UnityEngine;

[CreateAssetMenu(menuName = "Tool Behaviour/Seed")]
public class SeedBehaviour : ToolBehaviour
{
    public override bool PerformBehaviour()
    {
        //Apply tile effect (planted seed tile)
        //Remove one from the inventory (or do this somewhere else? check the object data for RemoveOnUse bool?)
        Debug.Log("Performing seedy behaviour");
        return true;
    }
}
