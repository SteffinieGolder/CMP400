using System.Collections.Generic;
using UnityEngine;

//Script which sets up player inventory. 

//Serialisable so it can be attached to player. 
[System.Serializable]
public class Inventory 
{
    [System.Serializable]
    //Slot class represents a slot in an inventory. 
    public class Slot
    {
        //Slot will have name of item stored in it, the amount, the total allowed and an icon. 
        public string itemName;
        public int count;
        public int maxAllowed;
        public Sprite icon;

        public Slot()
        {
            itemName = "";
            count = 0;
            maxAllowed = 99;
        }

        public bool IsEmpty
        {
            get
            {
                if(itemName == "" && count ==0)
                {
                    return true;
                }
                return false;
            }
        }

        //Function which checks if item can be added (player hasn't reached max capacity).
        public bool CanAddItem(string itemName)
        {
            if(this.itemName == itemName && count< maxAllowed)
            {
                return true;
            }

            return false;
        }

        //Function which fills slot with item data. 
        public void AddItem(Item item)
        {
            this.itemName = item.data.itemName;
            this.icon = item.data.icon;
            count++;
        }

        //Function which fills slot with item data. //////////////////////////////////////////////////////////////////FIX
        public void AddItem(string itemName, Sprite icon, int maxAllowed)
        {
            this.itemName = itemName;
            this.icon = icon;
            count++;
            this.maxAllowed = maxAllowed;
        }

        //Function which decreases the count of the removed item, and removes item data completely if count is 0.
        public void RemoveItem()
        {
            if(count>0)
            {
                count--;

                if(count == 0)
                {
                    icon = null;
                    itemName = "";
                }
            }
        }
    }

    //List of slots.
    public List<Slot> slots = new List<Slot>();
    public string inventoryName;

    //Constructor to initialise inventory with desired number of slots. 
    public Inventory(int numSlots, string name)
    {
        inventoryName = name; 
        for (int i = 0; i < numSlots; i++)
        {
            Slot slot = new Slot();
            slots.Add(slot);
        }
    }

    //Function which adds item data to inventory slot based on passed in item parameter.
    public void Add(Item item)
    {
        foreach(Slot slot in slots)
        {
            if(slot.itemName == item.data.itemName && slot.CanAddItem(item.data.itemName))
            {
                slot.AddItem(item);
                return;
            }
        }

        foreach(Slot slot in slots)
        {
            if(slot.itemName == "")
            {
                slot.AddItem(item);
                return;
            }
        }
    }

    //Function to remove item at desired index. 
    public void Remove(int index)
    {
        slots[index].RemoveItem();
    }


    //Function to remove item at desired index. 
    public void Remove(int index, int numToRemove)
    {
        if(slots[index].count >=numToRemove)
        {
            for(int i =0; i<numToRemove; i++)
            {
                Remove(index);
            }
        }
    }

    public void RemoveAllItemsOfType(Item itemToRemove)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].itemName == itemToRemove.data.itemName)
            {
                Remove(i, slots[i].count);
            }
        }
    }

    public void AddItemToAmount(int goalAmount, Item itemToAdd)
    {
        foreach (Slot slot in slots)
        {
            if (slot.itemName == itemToAdd.data.itemName && slot.CanAddItem(itemToAdd.data.itemName))
            {
                for (int i = slot.count; i < goalAmount; i++)
                {
                    slot.AddItem(itemToAdd);
                }

                return;
            }
        }

        foreach (Slot slot in slots)
        {
            if (slot.itemName == "")
            {
                for (int i = 0; i < goalAmount; i++)
                {
                    slot.AddItem(itemToAdd);
                }

                return;
            }
        }
    }

    public void MoveSlot(int fromIndex, int toIndex, Inventory toInventory, int numToMove = 1)
    {
        Slot fromSlot = slots[fromIndex];
        Slot toSlot = toInventory.slots[toIndex];

        if (fromSlot != null && toSlot != null)
        {
            if (toSlot.IsEmpty || toSlot.CanAddItem(fromSlot.itemName))
            {
                for (int i = 0; i < numToMove; i++)
                {
                    toSlot.AddItem(fromSlot.itemName, fromSlot.icon, fromSlot.maxAllowed);
                    fromSlot.RemoveItem();
                }
            }
        }
    }

    public int ReturnItemCount(Item item)
    {
        foreach (Slot slot in slots)
        {
            if (slot.itemName == item.data.itemName)
            {
                return slot.count;
            }
        }

        return 0;
    }
}
