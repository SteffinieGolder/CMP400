using System.Collections.Generic;
using UnityEngine;

//Script which controls the inventory UI. 

public class InventoryUI : MonoBehaviour
{
    //UI panel.
    public GameObject inventoryPanel;
    //Player ref.
    public Player player;
    //List of UI slots in inventory. 
    public List<SlotsUI> slots = new List<SlotsUI>();

    private void Update()
    {
        //Toggle inventory on/off when player presses TAB key. 
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    //Function which toggles inventory on/off by activating/deactivating UI panel element.
    public void ToggleInventory()
    {
        if(inventoryPanel.activeSelf == false)
        {
            inventoryPanel.SetActive(true);
            Refresh();
        }
        else
        {
            inventoryPanel.SetActive(false);
        }
    }

    //Function which refreshes inventory UI. 
    void Refresh()
    {
        if(slots.Count == player.inventory.slots.Count)
        {
            //Check each slot in the player inventory. If an item is present then set corresponding slot UI element to display the item information.  
            for (int i = 0; i<slots.Count; i++)
            {
                if(player.inventory.slots[i].itemName !="")
                {
                    slots[i].SetItem(player.inventory.slots[i]);
                }

                //Otherwise, remove any information.
                else
                {
                    slots[i].SetEmpty();
                }
            }
        }
    }

    //Function which removes item from player inventory.
    public void RemoveItem(int slotID)
    {
        //Call game manager to get details of item player has requested to drop. 
        Item itemToDrop = GameManager.instance.itemManager.GetItemByName(
            player.inventory.slots[slotID].itemName);

        //Check if item has been selected to drop. 
        if (itemToDrop != null)
        {
            //Tell player script to drop item.
            player.DropItem(itemToDrop);
            //Remove item from player inventory.
            player.inventory.Remove(slotID);
            //Refresh UI. 
            Refresh();
        }
    }
}
