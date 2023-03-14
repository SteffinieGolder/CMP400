using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tool Behaviour/Sword")]

public class SwordBehaviour : ToolBehaviour
{
    TileManager manager;
    TileData tileData;
    Vector3Int gridPos;
    ItemData itemData;

    public override bool CheckUseConditions(Vector2 position, ItemData item)
    {
        //CHECK ITS WITHIN A CERTAIN RANGE////////////////////////////////////////
        manager = GameManager.instance.tileManager;
        gridPos = manager.GetGridPosition(position, true, TileManager.tilemapOptions.GROUND);
        itemData = item;

        TileBase tile = manager.GetTileBase(gridPos, TileManager.tilemapOptions.GROUND);

        if (tile)
        {
            tileData = manager.GetTileData(tile);

            if (tileData)
            {
                if (tileData.canBeSliced)
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
        //Set char animation to holding the tool
        //Whenever player clicks to use, animate action
        //Remove tile
        GameManager.instance.characterManager.activePlayer.GetComponent<CharMovement>().animator.SetTrigger("slashTrigger");

        manager.ChangeTile(gridPos, null, TileManager.tilemapOptions.GROUND);

        Vector2 spawnLocation = new Vector2(GameManager.instance.characterManager.activePlayer.transform.position.x - 1.5f, GameManager.instance.characterManager.activePlayer.transform.position.y);

        Instantiate(itemData.itemToSpawn, GameManager.instance.characterManager.activePlayer.gameObject.GetComponent<CharMovement>().GetItemSpawnPos(), GameManager.instance.characterManager.activePlayer.transform.rotation);

        if (GameManager.instance.characterManager.char1IsActive)
        {
            if (GameManager.instance.taskManager.IsTaskPortionComplete(true, itemData.taskIndex))
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
            if (GameManager.instance.taskManager.IsTaskPortionComplete(false, itemData.taskIndex))
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
