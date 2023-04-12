using UnityEngine;

//Base class for tool behaviour scripts. 

public class ToolBehaviour : ScriptableObject
{
    //Function which checks use conditions unique to the tool. Returns true if the tool can be used and its conditions have been satisfied. 
    public virtual bool CheckUseConditions(Vector2 position, ItemData item)
    {
        Debug.LogWarning("CheckUseConditions is not applied");
        return true;
    }

    //Runs the specific tool behaviour. 
   public virtual bool PerformBehaviour()
    {
        Debug.LogWarning("PerformBehaviour is not applied");
        return true;
    }
}
