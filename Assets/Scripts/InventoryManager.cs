using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<string, Inventory> inventoryByName = new Dictionary<string, Inventory>();
    [Header("Backpack")]
    public Inventory backpack;
    public int backpackSlotCount;

    [Header("Toolbar")]
    public Inventory toolbar;
    public int toolbarSlotCount;

    [Header("Storage")]
    public Inventory storage;
    public int storageSlotCount;

    private void Awake()
    {
        backpack = new Inventory(backpackSlotCount, backpack.inventoryName);
        toolbar = new Inventory(toolbarSlotCount, toolbar.inventoryName);
        storage = new Inventory(storageSlotCount, storage.inventoryName);

        inventoryByName.Add(backpack.inventoryName, backpack);
        inventoryByName.Add(toolbar.inventoryName, toolbar);
        inventoryByName.Add(storage.inventoryName, storage);
    }

    public void Add(string inventoryName, Item item)
    {
        if(inventoryByName.ContainsKey(inventoryName))
        {
            inventoryByName[inventoryName].Add(item);
        }
    }

    public Inventory GetInventoryByName(string inventoryName)
    {
        if(inventoryByName.ContainsKey(inventoryName))
        {
            return inventoryByName[inventoryName];
        }

        return null;
    }

    public void InitialiseInventoryWithItems(Dictionary<Item, int> items, string nameOfInventory)
    {
        foreach(Item item in items.Keys)
        {
            int itemAmount = items[item];

            GetInventoryByName(nameOfInventory).AddItemToAmount(itemAmount, item);
        }
           
    }

    public int DoesStorageContainEndItems(Item item)
    {
        return storage.ReturnItemCount(item);
    }
}
