using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Should I make this so its the canvas thats getting disabled rather than panel? 
public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Player player;
    public List<SlotsUI> slots = new List<SlotsUI>();

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

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

    void Refresh()
    {
        if(slots.Count == player.inventory.slots.Count)
        {
            for (int i = 0; i<slots.Count; i++)
            {
                if(player.inventory.slots[i].itemName !="")
                {
                    slots[i].SetItem(player.inventory.slots[i]);
                }

                else
                {
                    slots[i].SetEmpty();
                }
            }
        }
    }

    public void RemoveItem(int slotID)
    {
        Item itemToDrop = GameManager.instance.itemManager.GetItemByName(
            player.inventory.slots[slotID].itemName);

        Debug.Log("Dropping");

        if (itemToDrop != null)
        {
            player.DropItem(itemToDrop);
            player.inventory.Remove(slotID);
            Refresh();
        }
    }
}
