using System.Collections.Generic;
using UnityEngine;

//Script which tracks items in game. Used to store item reference for instantiation when it is dropped from inventory. 

public class ItemManager : MonoBehaviour
{
    //Array of each item in game. 
    public Item[] items;
    [SerializeField] GameObject hoeSpawnPosition;

    //Dictionary holding name and item object.
    private Dictionary<string, Item> nameToItemDict = new Dictionary<string, Item>();

    private Item equippedItem;
    private SlotsUI equippedSlot;
    private bool once = false;

    private void Awake()
    {
        //Run AddItem func for each item in array. 
        foreach (Item item in items)
        {
            AddItem(item);
            item.data.currentIndex = 0;
        }
    }

    private void Update()
    {
        if (!once)
        {
            if (GameManager.instance.characterManager.char1IsActive)
            {
                if (GameManager.instance.taskManager.isFishingComplete)
                {
                    Instantiate(GetItemByName("Hoe"), hoeSpawnPosition.transform.position, Quaternion.identity);
                    once = true;
                }
            }
        }

        if (Time.timeScale != 0)
        {
            if (equippedItem)
            {
                if (equippedItem.data.itemName != "Milk")
                {
                    if (GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().currentEmote.doesCharAcceptTask)
                    {
                        {
                            RunTaskAccepted();
                        }
                    }
                    else
                    {
                        RunTaskRejected();
                    }
                }

                else
                {
                    DrinkMilk();
                }
            }
        }
    }

    private void RunTaskAccepted()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!UIManager.isPointerOnToggleUI && !UIManager.isPointerOnConstantUI)
            {
                Vector2 pos = Input.mousePosition;
                if (equippedItem.data.isEquippable)
                {
                    if (equippedItem.data.toolBehaviourScript.CheckUseConditions(pos, equippedItem.data))
                    {
                        if (equippedItem.data.toolBehaviourScript.PerformBehaviour())
                        {
                            //Remove item from player inventory.
                            Inventory inventory = equippedSlot.inventory;
                            inventory.Remove(equippedSlot.slotID);
                            GameManager.instance.uiManager.RefreshInventoryUI(inventory.inventoryName);

                            if (equippedSlot.quantityText.text == "")
                            {
                                equippedItem = null;
                            }
                        }
                    }
                }
            }
        }
    }

    private void RunTaskRejected()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!UIManager.isPointerOnToggleUI && !UIManager.isPointerOnConstantUI)
            {
                Vector2 pos = Input.mousePosition;
                if (equippedItem.data.isEquippable)
                {
                    if (equippedItem.data.toolBehaviourScript.CheckUseConditions(pos, equippedItem.data))
                    {
                        CharBehaviourBase behaviour = GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>();

                        behaviour.DisplayRejectDialogue();
                    }
                }
            }
        }
    }

    private void DrinkMilk()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!UIManager.isPointerOnToggleUI && !UIManager.isPointerOnConstantUI)
            {
                Vector2 pos = Input.mousePosition;
                if (equippedItem.data.isEquippable)
                {
                    if (equippedItem.data.toolBehaviourScript.CheckUseConditions(pos, equippedItem.data))
                    {
                        if (equippedItem.data.toolBehaviourScript.PerformBehaviour())
                        {
                            //Remove item from player inventory.
                            Inventory inventory = equippedSlot.inventory;
                            inventory.Remove(equippedSlot.slotID);
                            GameManager.instance.uiManager.RefreshInventoryUI(inventory.inventoryName);

                            if (equippedSlot.quantityText.text == "")
                            {
                                equippedItem = null;
                            }
                        }
                    }
                }
            }
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

    public void EquipItem(string equipName, SlotsUI slot)
    {
        if (nameToItemDict.ContainsKey(equipName))
        {
            equippedItem = nameToItemDict[equipName];
            equippedSlot = slot;
        }

        else
        {
            equippedItem = null;
        }
    }

    public void ResetEquippedItem()
    {
        equippedItem = null;
    }
}
