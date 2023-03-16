using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script which controls the inventory UI. 

public class InventoryUI : MonoBehaviour
{
    public string inventoryName;
    public GameObject owningCharacter;

    //List of UI slots in inventory. 
    public List<SlotsUI> slots = new List<SlotsUI>();

    [SerializeField] private Canvas canvas;

    private Inventory inventory;
    private Player owningCharScript;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    private void Start()
    {
        inventory = owningCharacter.GetComponent<InventoryManager>().GetInventoryByName(inventoryName);
        owningCharScript = owningCharacter.GetComponent<Player>();
        SetupSlots();
        Refresh();
    }

    //Function which refreshes inventory UI. 
    public void Refresh()
    {
        if (inventory != null)
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
    }

    //Function which removes item from player inventory.
    public void RemoveItem()
    {
        //Call game manager to get details of item player has requested to drop. 
        Item itemToDrop = GameManager.instance.itemManager.GetItemByName(inventory.slots[UIManager.draggedSlot.slotID].itemName);

        //Check if item has been selected to drop. 
        if (itemToDrop != null)
        {
            //THIS IS BAD/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (inventory.inventoryName == "Storage_C1" || inventory.inventoryName == "Storage_C2")
            {
                slots[UIManager.draggedSlot.slotID].ratingImage.SetActive(false);
            }

            if (UIManager.dragSingle)
            {
                //Tell player script to drop item.
                owningCharScript.DropItem(itemToDrop);
                //Remove item from player inventory.
                inventory.Remove(UIManager.draggedSlot.slotID);
            }

            else
            {
                //Tell player script to drop item.
                owningCharScript.DropItem(itemToDrop, inventory.slots[UIManager.draggedSlot.slotID].count);
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
    }

    public void SlotDrag()
    {
        MoveToMousePosition(UIManager.draggedIcon.gameObject);
    }

    public void SlotEndDrag()
    {
        Destroy(UIManager.draggedIcon.gameObject);
        UIManager.draggedIcon = null;
    }

    public void SlotDrop(SlotsUI slot)
    {
        if (slot)
        {
            if (UIManager.draggedSlot)
            {
                //THIS IS BAD/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (slot.inventory.inventoryName == "Storage_C1" || slot.inventory.inventoryName == "Storage_C2")
                {
                    Item itemToGrade = GameManager.instance.itemManager.GetItemByName(UIManager.draggedSlot.inventory.slots[UIManager.draggedSlot.slotID].itemName);

                    if (itemToGrade)
                    {
                        if (itemToGrade.data.itemName == "Fish" || itemToGrade.data.itemName == "Strawberry" || itemToGrade.data.itemName == "Tomato" || itemToGrade.data.itemName == "Carrot")
                        {
                            if(GameManager.instance.characterManager.char1IsActive)
                            {
                                slot.ratingImage.GetComponent<Image>().sprite = itemToGrade.data.ADHDGradeImage;

                            }

                            else
                            {
                                slot.ratingImage.GetComponent<Image>().sprite = itemToGrade.data.NTGradeImage;
                            }

                            slot.ratingImage.SetActive(true);
                        }
                    }
                }

                if (UIManager.dragSingle)
                {
                    UIManager.draggedSlot.inventory.MoveSlot(UIManager.draggedSlot.slotID, slot.slotID, slot.inventory);

                    if (UIManager.draggedSlot.inventory.inventoryName == "Storage_C1" || UIManager.draggedSlot.inventory.inventoryName == "Storage_C2")
                    {
                        if (UIManager.draggedSlot.inventory.slots[UIManager.draggedSlot.slotID].IsEmpty)
                        {
                            UIManager.draggedSlot.ratingImage.SetActive(false);
                        }
                    }
                }

                else
                {
                    UIManager.draggedSlot.inventory.MoveSlot(UIManager.draggedSlot.slotID, slot.slotID, slot.inventory,
                        UIManager.draggedSlot.inventory.slots[UIManager.draggedSlot.slotID].count);

                    //THIS IS BAD/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (UIManager.draggedSlot.inventory.inventoryName == "Storage_C1" || UIManager.draggedSlot.inventory.inventoryName == "Storage_C2")
                    {
                        UIManager.draggedSlot.ratingImage.SetActive(false);
                    }
                }

                GameManager.instance.uiManager.RefreshAll();
                UIManager.draggedSlot = null;
            }
        }
    }

    private void MoveToMousePosition(GameObject toMove)
    {
        if (canvas != null)
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

        foreach (SlotsUI slot in slots)
        {
            slot.slotID = counter;
            counter++;
            slot.inventory = inventory;
        }
    }

   /* public void CheckSlotsForGrading(Item itemToGrade, bool isChar1)
    {
        // if (inventoryNameForGrading == "Storage_C1" || inventoryNameForGrading == "Storage_C2")
        // {
        // if (inventoryName == inventoryNameForGrading)
        // {
        foreach (SlotsUI slot in slots)
        {
            if (slot.itemIcon == itemToGrade.data.icon)
            {
                if (isChar1)
                {
                    slot.ratingImage.GetComponent<Image>().sprite = itemToGrade.data.ADHDGradeImage;
                }
                else
                {
                    slot.ratingImage.GetComponent<Image>().sprite = itemToGrade.data.NTGradeImage;
                }

                slot.ratingImage.SetActive(true);
            }
            //     }
        }
        //  }
    }*/
}
