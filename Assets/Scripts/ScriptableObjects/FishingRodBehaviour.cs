using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tool Behaviour/Fishing Rod")]

public class FishingRodBehaviour : ToolBehaviour
{
    TileManager manager;
    TileData tileData;
    Vector3Int gridPos;
    ItemData itemData;
    public override bool CheckUseConditions(Vector2 position, ItemData item)
    {
        manager = GameManager.instance.tileManager;
        gridPos = manager.GetGridPosition(position, true, TileManager.tilemapOptions.GROUND);
        itemData = item;

        TileBase tile = manager.GetTileBase(gridPos, TileManager.tilemapOptions.GROUND);

        //Check if the tile is in range of the fishing rod. 

        if (tile)
        {
            tileData = manager.GetTileData(tile);

            if (tileData)
            {
                if (tileData.canFish)
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
        //Put fish in inventory
        //Change tile
        GameManager.instance.characterManager.activePlayer.GetComponent<CharMovement>().animator.SetTrigger("fishTrigger");

        manager.ChangeTile(gridPos, itemData.tileToChangeTo, TileManager.tilemapOptions.GROUND);

        Instantiate(itemData.itemToSpawn, GameManager.instance.characterManager.activePlayer.transform.position,
            GameManager.instance.characterManager.activePlayer.transform.rotation);

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
