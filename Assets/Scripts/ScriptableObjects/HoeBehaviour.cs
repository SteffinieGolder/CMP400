using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tool Behaviour/Hoe")]

public class HoeBehaviour : ToolBehaviour
{
    TileManager manager;
    TileData tileData;
    Vector3Int gridPos;
    Vector3Int gridPos2;
    ItemData itemData;

    public override bool CheckUseConditions(Vector2 position, ItemData item)
    {
        manager = GameManager.instance.tileManager;
        gridPos = manager.GetGridPosition(position, true, TileManager.tilemapOptions.BACKGROUND);
        gridPos2 = manager.GetGridPosition(position, true, TileManager.tilemapOptions.GROUND);
        itemData = item;

        TileBase tile = manager.GetTileBase(gridPos, TileManager.tilemapOptions.BACKGROUND);
        TileBase tile2 = manager.GetTileBase(gridPos2, TileManager.tilemapOptions.GROUND);

        if (!tile2)
        {
            if (tile)
            {
                tileData = manager.GetTileData(tile);

                if (tileData)
                {
                    if (tileData.isPlowable)
                    {
                        if (Vector3.Distance(GameManager.instance.characterManager.activePlayer.transform.position, manager.GetWorldPosition(gridPos, TileManager.tilemapOptions.GROUND)) <= itemData.interactRange)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public override bool PerformBehaviour()
    {
        //Set char animation to holding the tool
        //Whenever player clicks to use, animate action
        //Set tile to plowed tile. 
        GameManager.instance.characterManager.activePlayer.GetComponent<CharMovement>().animator.SetTrigger("hoeTrigger");
        manager.ChangeTile(gridPos, itemData.tileToChangeTo, TileManager.tilemapOptions.BACKGROUND);

        if (GameManager.instance.characterManager.char1IsActive)
        {
            if (GameManager.instance.taskManager.IsTaskComplete(true, itemData.taskIndex))
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
            if (GameManager.instance.taskManager.IsTaskComplete(false, itemData.taskIndex))
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
