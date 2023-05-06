using System.Collections.Generic;
using UnityEngine;

//Script which tracks items in game. Used to store item reference for instantiation when it is dropped from inventory. 
//Code adapted from this series by Jacquelynne Hei: https://www.youtube.com/watch?v=ZPYrdKMDsGI&list=PL4PNgDjMajPN51E5WzEi7cXzJ16BCHZXl&ab_channel=GameDevwithJacquelynneHei

public class ItemManager : MonoBehaviour
{
    //Array of each item in game. 
    public Item[] items;
    [SerializeField] GameObject hoeSpawnPosition;

    //Dictionary holding name and item object.
    private Dictionary<string, Item> nameToItemDict = new Dictionary<string, Item>();

    //Variables for an equipped item;
    private Item equippedItem;
    private SlotsUI equippedSlot;
    private bool once = false;

    private void Awake()
    {
        //Add items to dictionary.
        foreach (Item item in items)
        {
            AddItem(item);
            item.data.currentIndex = 0;
        }
    }

    private void Update()
    {
        //Spawn the hoe object in the environment after the fishing task is complete. Do this once.
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

        //Do this if the game isn't paused.
        if (Time.timeScale != 0)
        {
            if (equippedItem)
            {
                //Check that the milk object isn't equipped.
                if (equippedItem.data.itemName != "Milk")
                {
                    //Check if the character will accept the task (they will refuse if they're too tired).
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

    //Run the task behaviour.
    private void RunTaskAccepted()
    {
        //Checks if the user has clicked and their not on a UI element.
        if (Input.GetMouseButtonDown(0))
        {
            if (!UIManager.isPointerOnToggleUI && !UIManager.isPointerOnConstantUI)
            {
                Vector2 pos = Input.mousePosition;
                //Checks if current toolbar item is equippable.
                if (equippedItem.data.isEquippable)
                {
                    //Checks if the item can be used based on its specific conditions.
                    if (equippedItem.data.toolBehaviourScript.CheckUseConditions(pos, equippedItem.data))
                    {
                        //Performs the behaviour based on the equipped tool. If this returns true then the item should be removed from the inventory after use.
                        if (equippedItem.data.toolBehaviourScript.PerformBehaviour())
                        {
                            //Remove item from player inventory.
                            Inventory inventory = equippedSlot.inventory;
                            inventory.Remove(equippedSlot.slotID);
                            GameManager.instance.uiManager.RefreshInventoryUI(inventory.inventoryName);

                            //Reset the equipped item if the user has run out.
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

    //Reject the task. Same as above but doesn't run the behaviour, it just displays dialogue.
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

    //Drink the milk (coffee).
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

    //Equips the user with the desired item.
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

    //Resets the equipped item.
    public void ResetEquippedItem()
    {
        equippedItem = null;
    }
}
