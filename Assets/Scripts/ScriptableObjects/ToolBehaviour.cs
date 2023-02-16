using UnityEngine;

public class ToolBehaviour : ScriptableObject
{
    public virtual bool CheckUseConditions(Vector2 position)
    {
        Debug.LogWarning("CheckUseConditions is not applied");
        return true;
    }

   public virtual bool PerformBehaviour()
    {
        Debug.LogWarning("PerformBehaviour is not applied");
        return true;
    }
}
