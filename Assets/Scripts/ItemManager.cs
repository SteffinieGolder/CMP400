using System.Collections.Generic;
using UnityEngine;

//Script which tracks items in game. Used to store item reference for instantiation when it is dropped from inventory. 

public class ItemManager : MonoBehaviour
{
    //Array of each item in game. 
    public Item[] items;

    //Dictionary holding name and item object.
    private Dictionary<string, Item> nameToItemDict = new Dictionary<string, Item>();

    private void Awake()
    {
        //Run AddItem func for each item in array. 
        foreach(Item item in items)
        {
            AddItem(item);
        }
    }

    //Function which populates item dictionary with item data if it doesn't already exist. 
    private void AddItem(Item item)
    {
        if (!nameToItemDict.ContainsKey(item.data.itemName))
        {
            nameToItemDict.Add(item.data.itemName, item);
        }
    }

    //Function which returns name of item if it exists.
    public Item GetItemByName(string key)
    {
        if(nameToItemDict.ContainsKey(key))
        {
            return nameToItemDict[key];
        }

        return null;
    }
}