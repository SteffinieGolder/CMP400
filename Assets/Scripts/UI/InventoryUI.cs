using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script which controls the inventory UI. 

public class InventoryUI : MonoBehaviour
{
    //UI panel.
    public GameObject inventoryPanel;
    //Player ref.
    public Player player;
    //List of UI slots in inventory. 
    public List<SlotsUI> slots = new List<SlotsUI>();

    [SerializeField] private Canvas canvas;
    private SlotsUI draggedSlot;
    private Image draggedIcon;
    private bool draggedSingle = false;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
    }

    private void Update()
    {
        //Toggle inventory on/off when player presses TAB key. 
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            draggedSingle = true;
        }
        else
        {
            draggedSingle = false;
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
    public void RemoveItem()
    {
        //Call game manager to get details of item player has requested to drop. 
        Item itemToDrop = GameManager.instance.itemManager.GetItemByName(
            player.inventory.slots[draggedSlot.slotID].itemName);

        //Check if item has been selected to drop. 
        if (itemToDrop != null)
        {
            if (draggedSingle)
            {
                //Tell player script to drop item.
                player.DropItem(itemToDrop);
                //Remove item from player inventory.
                player.inventory.Remove(draggedSlot.slotID);
            }

            else
            {
                //Tell player script to drop item.
                player.DropItem(itemToDrop, player.inventory.slots[draggedSlot.slotID].count);
                //Remove item from player inventory.
                player.inventory.Remove(draggedSlot.slotID, player.inventory.slots[draggedSlot.slotID].count);
            }
           
            //Refresh UI. 
            Refresh();
        }

        draggedSlot = null;
    }

    public void SlotBeginDrag(SlotsUI slot)
    {
        draggedSlot = slot;
        draggedIcon = Instantiate(draggedSlot.itemIcon);
        draggedIcon.transform.SetParent(canvas.transform);
        draggedIcon.raycastTarget = false;
        draggedIcon.rectTransform.sizeDelta = new Vector2(50, 50);
        MoveToMousePosition(draggedIcon.gameObject);
        //Debug.Log("Start Dragging: " + draggedSlot.name);
    }

    public void SlotDrag()
    {
        MoveToMousePosition(draggedIcon.gameObject);
       // Debug.Log("Dragging: " + draggedSlot.name);
    }

    public void SlotEndDrag()
    {
        Destroy(draggedIcon.gameObject);
        draggedIcon = null;
       // Debug.Log("Done Dragging: " + draggedSlot.name);
    }

    public void SlotDrop(SlotsUI slot)
    {
       // Debug.Log("Dropped " + draggedSlot.name + " on " + slot.name);
    }

    private void MoveToMousePosition(GameObject toMove)
    {
        if(canvas!=null)
        {
            Vector2 position;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                Input.mousePosition, null, out position);

            toMove.transform.position = canvas.transform.TransformPoint(position);
        }
    }
}
