using UnityEngine;

public class ToolBehaviour : ScriptableObject
{
   public virtual bool PerformBehaviour()
    {
        Debug.LogWarning("PerformBehaviour is not applied");
        return true;
    }
}
