using System.Collections.Generic;
using UnityEngine;

//Script which sets up player inventory. 

//Serialisable so it can be attached to player. 
[System.Serializable]
public class Inventory 
{
    [System.Serializable]
    //Slot class reprents a slot in the inventory. 
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

        //Function which checks if item can be added (player hasn't reached max capacity).
        public bool CanAddItem()
        {
            if(count< maxAllowed)
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

    //Constructor to initialise inventory with desired number of slots. 
    public Inventory(int numSlots)
    {
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
            if(slot.itemName == item.data.itemName && slot.CanAddItem())
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
}
