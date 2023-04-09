using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Tool Behaviour/Watering Can")]

public class WateringCanBehaviour : ToolBehaviour
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
        TileBase backgroundTile = manager.GetTileBase(gridPos, TileManager.tilemapOptions.BACKGROUND);

        if (backgroundTile)
        {
            if(backgroundTile.name == itemData.tileToChangeTo.name)
            {
                return false;
            }
        }

        if (tile)
        {
            tileData = manager.GetTileData(tile);

            if (tileData)
            {
                if (tileData.isWaterable)
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
            if (GameManager.instance.taskManager.IsTaskPortionComplete(true, itemData.taskIndex))
            {
                if (GameManager.instance.taskManager.IsTaskTotallyComplete(true, itemData.taskIndex))
                {
                    //Fade out UI. 
                    GameManager.instance.uiManager.FadeInOrOut(true);

                    //Change all tiles to seeds.
                    GameManager.instance.tileManager.ChangeAllPatchTiles(GameManager.instance.tileManager.GetAllPatchGridPositions(itemData.TilePatchPositions,
                        TileManager.tilemapOptions.GROUND), TileManager.tilemapOptions.GROUND, itemData.patchTile);

                    //Remove all seeds from the inventory.
                    Item seedItem1 = GameManager.instance.itemManager.GetItemByName("Carrot Seeds");

                    if (seedItem1)
                    {
                        GameManager.instance.characterManager.activePlayer.inventoryManager.backpack.RemoveAllItemsOfType(seedItem1);
                        GameManager.instance.characterManager.activePlayer.inventoryManager.toolbar.RemoveAllItemsOfType(seedItem1);
                        GameManager.instance.uiManager.RefreshAll();
                    }

                    //Add any remaining crops to the inventory
                    Item carrot = GameManager.instance.itemManager.GetItemByName("Carrot");
                    Item strawb = GameManager.instance.itemManager.GetItemByName("Strawberry");
                    Item tomato = GameManager.instance.itemManager.GetItemByName("Tomato");

                    if (carrot && strawb && tomato)
                    {
                        int carrotsInStorage = GameManager.instance.characterManager.activePlayer.inventoryManager.storage.ReturnItemCount(carrot);
                        int strawbsInStorage = GameManager.instance.characterManager.activePlayer.inventoryManager.storage.ReturnItemCount(strawb);
                        int tomatsInStorage = GameManager.instance.characterManager.activePlayer.inventoryManager.storage.ReturnItemCount(tomato);

                        GameManager.instance.characterManager.activePlayer.inventoryManager.backpack.AddItemToAmount(20 - carrotsInStorage, carrot);
                        GameManager.instance.characterManager.activePlayer.inventoryManager.backpack.AddItemToAmount(9 - strawbsInStorage, strawb);
                        GameManager.instance.characterManager.activePlayer.inventoryManager.backpack.AddItemToAmount(11 - tomatsInStorage, tomato);
                    }

                    //Change some tiles to watered. 
                    List<Vector2> lessPositions = new List<Vector2>();
                    for (int i = 0; i < itemData.TilePatchPositions.Count/2; i++)
                    {
                        lessPositions.Add(itemData.TilePatchPositions[i]);
                    }

                    GameManager.instance.tileManager.ChangeAllPatchTiles(GameManager.instance.tileManager.GetAllPatchGridPositions(lessPositions,
                       TileManager.tilemapOptions.BACKGROUND), TileManager.tilemapOptions.BACKGROUND, itemData.tileToChangeTo);

              
                    //Show Dialogue lines.
                    GameManager.instance.uiManager.SetDialogueData(charData.GetDialogueGroup(itemData.ADHDDialogueGroupIndexes[0]).dialogueLines, 
                        charData.GetDialogueGroup(itemData.ADHDDialogueGroupIndexes[0]).expressionTypes);

                    GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateBehaviour(itemData.ADHDTimeValue, itemData.ADHDMultiplier, false);

                }

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

                }
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
