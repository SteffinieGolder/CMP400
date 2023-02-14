using System.Collections.Generic;
using UnityEngine;

//Script which controls the inventory UI. 

public class InventoryUI : MonoBehaviour
{
    public string inventoryName;
    
    //List of UI slots in inventory. 
    public List<SlotsUI> slots = new List<SlotsUI>();

    [SerializeField] private Canvas canvas;
   
    private Inventory inventory;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
    }

    private void Start()
    {
        inventory = GameManager.instance.player.inventory.GetInventoryByName(inventoryName);
        SetupSlots();
        Refresh();
    }

    //Function which refreshes inventory UI. 
    public void Refresh()
    {
        if (slots.Count == inventory.slots.Count)
        {
            //Check each slot in the player inventory. If an item is present then set corresponding slot UI element to display the item information.  
            for (int i = 0; i < slots.Count; i++)
            {
                if (inventory.slots[i].itemName != "")
                {
                    slots[i].SetItem(inventory.slots[i]);
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
        Item itemToDrop = GameManager.instance.itemManager.GetItemByName(inventory.slots[UIManager.draggedSlot.slotID].itemName);

        //Check if item has been selected to drop. 
        if (itemToDrop != null)
        {
            if (UIManager.dragSingle)
            {
                //Tell player script to drop item.
                GameManager.instance.player.DropItem(itemToDrop);
                //Remove item from player inventory.
                inventory.Remove(UIManager.draggedSlot.slotID);
            }

            else
            {
                //Tell player script to drop item.
                GameManager.instance.player.DropItem(itemToDrop, inventory.slots[UIManager.draggedSlot.slotID].count);
                //Remove item from player inventory.
                inventory.Remove(UIManager.draggedSlot.slotID, inventory.slots[UIManager.draggedSlot.slotID].count);
            }
           
            //Refresh UI. 
            Refresh();
        }

        UIManager.draggedSlot = null;
    }

    public void SlotBeginDrag(SlotsUI slot)
    {
        UIManager.draggedSlot = slot;
        UIManager.draggedIcon = Instantiate(UIManager.draggedSlot.itemIcon);
        UIManager.draggedIcon.transform.SetParent(canvas.transform);
        UIManager.draggedIcon.raycastTarget = false;
        UIManager.draggedIcon.rectTransform.sizeDelta = new Vector2(50, 50);
        MoveToMousePosition(UIManager.draggedIcon.gameObject);
        //Debug.Log("Start Dragging: " + draggedSlot.name);
    }

    public void SlotDrag()
    {
        MoveToMousePosition(UIManager.draggedIcon.gameObject);
       // Debug.Log("Dragging: " + draggedSlot.name);
    }

    public void SlotEndDrag()
    {
        Destroy(UIManager.draggedIcon.gameObject);
        UIManager.draggedIcon = null;
       // Debug.Log("Done Dragging: " + draggedSlot.name);
    }

    public void SlotDrop(SlotsUI slot)
    {
        if (UIManager.dragSingle)
        {
            // Debug.Log("Dropped " + draggedSlot.name + " on " + slot.name);
            UIManager.draggedSlot.inventory.MoveSlot(UIManager.draggedSlot.slotID, slot.slotID, slot.inventory);
        }
        else
        {
            UIManager.draggedSlot.inventory.MoveSlot(UIManager.draggedSlot.slotID, slot.slotID, slot.inventory,
                UIManager.draggedSlot.inventory.slots[UIManager.draggedSlot.slotID].count);

        }
        GameManager.instance.uiManager.RefreshAll();
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

    void SetupSlots()
    {
        int counter = 0;

        foreach(SlotsUI slot in slots)
        {
            slot.slotID = counter;
            counter++;
            slot.inventory = inventory;
        }
    }
}
