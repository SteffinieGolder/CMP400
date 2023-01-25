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
            SetUp();
        }
        else
        {
            inventoryPanel.SetActive(false);
        }
    }

    void SetUp()
    {
        if(slots.Count == player.inventory.slots.Count)
        {
            for (int i = 0; i<slots.Count; i++)
            {
                if(player.inventory.slots[i].type !=CollectableType.NONE)
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
}
