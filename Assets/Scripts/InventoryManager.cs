using System.Collections.Generic;
using UnityEngine;

//Script which initialises and manages the character inventories.
//Code adapted from this series by Jacquelynne Hei: https://www.youtube.com/watch?v=ZPYrdKMDsGI&list=PL4PNgDjMajPN51E5WzEi7cXzJ16BCHZXl&ab_channel=GameDevwithJacquelynneHei

public class InventoryManager : MonoBehaviour
{
    //Dictionary of inventories.
    public Dictionary<string, Inventory> inventoryByName = new Dictionary<string, Inventory>();

    //Different inventory types.
    [Header("Backpack")]
    //Inventory type.
    public Inventory backpack;
    //Amount of slots in this inventory.
    public int backpackSlotCount;

    [Header("Toolbar")]
    public Inventory toolbar;
    public int toolbarSlotCount;

    [Header("Storage")]
    public Inventory storage;
    public int storageSlotCount;

    private void Awake()
    {
        //Initialise inventories with slots and a name.
        backpack = new Inventory(backpackSlotCount, backpack.inventoryName);
        toolbar = new Inventory(toolbarSlotCount, toolbar.inventoryName);
        storage = new Inventory(storageSlotCount, storage.inventoryName);

        //Add inventories to dictionary.
        inventoryByName.Add(backpack.inventoryName, backpack);
        inventoryByName.Add(toolbar.inventoryName, toolbar);
        inventoryByName.Add(storage.inventoryName, storage);
    }

    //Add item if the inventory exists.
    public void Add(string inventoryName, Item item)
    {
        if(inventoryByName.ContainsKey(inventoryName))
        {
            inventoryByName[inventoryName].Add(item);
        }
    }

    //Return the inventory.
    public Inventory GetInventoryByName(string inventoryName)
    {
        if(inventoryByName.ContainsKey(inventoryName))
        {
            return inventoryByName[inventoryName];
        }

        return null;
    }

    //Add a desired amount of an item to the inventory if it exists. Used to initialise inventory with items at the start of the game.
    public void InitialiseInventoryWithItems(Dictionary<Item, int> items, string nameOfInventory)
    {
        foreach(Item item in items.Keys)
        {
            int itemAmount = items[item];

            GetInventoryByName(nameOfInventory).AddItemToAmount(itemAmount, item);
        }
           
    }

    //Return the amount of an item the inventory contains.
    public int DoesStorageContainEndItems(Item item)
    {
        return storage.ReturnItemCount(item);
    }
}
