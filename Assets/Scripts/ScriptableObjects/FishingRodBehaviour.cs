using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tool Behaviour/Fishing Rod")]

public class FishingRodBehaviour : ToolBehaviour
{
    TileManager manager;
    TileData tileData;
    Vector3Int gridPos;
    ItemData itemData;
    CharacterData charData;

    public override bool CheckUseConditions(Vector2 position, ItemData item)
    {
        manager = GameManager.instance.tileManager;
        gridPos = manager.GetGridPosition(position, true, TileManager.tilemapOptions.GROUND);
        itemData = item;
        charData = GameManager.instance.characterManager.activePlayer.charData;

        TileBase tile = manager.GetTileBase(gridPos, TileManager.tilemapOptions.GROUND);

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
        if (GameManager.instance.characterManager.char1IsActive)
        {
            GameManager.instance.taskManager.fishTaskCounter++;

            int current = GameManager.instance.taskManager.fishTaskCounter;

            if (!GameManager.instance.taskManager.hasFishingStarted)
            {
                GameManager.instance.taskManager.hasFishingStarted = true;
            }

            if (current == 2 || current == 3 || current == 4 || current == 7 || current == 9 || current == 10)
            {
                //Show Dialogue lines.
                GameManager.instance.uiManager.SetDialogueData(charData.GetDialogueGroup(itemData.ADHDDialogueGroupIndexes[itemData.currentIndex]).dialogueLines,
                    charData.GetDialogueGroup(itemData.ADHDDialogueGroupIndexes[itemData.currentIndex]).expressionTypes);

                if (current == 10)
                {
                    GameManager.instance.taskManager.isFishingComplete = true;
                    GameManager.instance.taskManager.hasFishingStarted = false;
                }

                itemData.currentIndex++;
                manager.ChangeTile(gridPos, itemData.tileToChangeTo, TileManager.tilemapOptions.GROUND);

            }

            else
            {
                RunBehaviour();
            }
        }

        else
        {
            RunBehaviour();
        }

        //Return false if item is reusable
        return false;
    }

    void RunBehaviour()
    {
        GameManager.instance.characterManager.activePlayer.GetComponent<CharMovement>().animator.SetTrigger("fishTrigger");

        manager.ChangeTile(gridPos, itemData.tileToChangeTo, TileManager.tilemapOptions.GROUND);

        Instantiate(itemData.itemToSpawn, GameManager.instance.characterManager.activePlayer.gameObject.GetComponent<CharMovement>().GetItemSpawnPos(), GameManager.instance.characterManager.activePlayer.transform.rotation);

        if (GameManager.instance.characterManager.char1IsActive)
        {
            if (GameManager.instance.taskManager.IsTaskPortionComplete(true, itemData.taskIndex))
            {
                GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateBehaviour(itemData.ADHDTimeValue, itemData.ADHDMultiplier, false);
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
                GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateBehaviour(itemData.NTTimeValue, itemData.NTMultiplier, false);

                if (GameManager.instance.taskManager.IsTaskTotallyComplete(false, itemData.taskIndex))
                {
                    //TaskComplete
                }
            }
            else
            {
                //NOT COMPLETE
            }
        }
    }
}
