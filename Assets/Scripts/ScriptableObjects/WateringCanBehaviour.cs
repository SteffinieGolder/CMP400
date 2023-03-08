using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tool Behaviour/Watering Can")]

public class WateringCanBehaviour : ToolBehaviour
{
    TileManager manager;
    TileData tileData;
    Vector3Int gridPos;
    ItemData itemData;
    public override bool CheckUseConditions(Vector2 position, ItemData item)
    {
        manager = GameManager.instance.tileManager;
        gridPos = manager.GetGridPosition(position, true, TileManager.tilemapOptions.BACKGROUND);
        itemData = item;

        TileBase tile = manager.GetTileBase(gridPos, TileManager.tilemapOptions.BACKGROUND);
        
        if (tile)
        {
            tileData = manager.GetTileData(tile);

            if (tileData)
            {
                if (tileData.isPlantable)
                {
                    if (Vector3.Distance(GameManager.instance.characterManager.activePlayer.transform.position, manager.GetWorldPosition(gridPos, TileManager.tilemapOptions.GROUND)) <= itemData.interactRange)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public override bool PerformBehaviour()
    {
        //Do any animations
        GameManager.instance.characterManager.activePlayer.GetComponent<CharMovement>().animator.SetTrigger("waterTrigger");

        //Change tile. 
        manager.ChangeTile(gridPos, itemData.tileToChangeTo, TileManager.tilemapOptions.BACKGROUND);

        if (GameManager.instance.characterManager.char1IsActive)
        {
            if (GameManager.instance.taskController.IsTaskComplete(true, itemData.taskIndex))
            {
                GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateTime(itemData.timeValue);
            }
            else
            {
                //NOT COMPLETE
            }
        }

        else
        {
            if (GameManager.instance.taskController.IsTaskComplete(false, itemData.taskIndex))
            {
                GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateTime(itemData.timeValue);
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
