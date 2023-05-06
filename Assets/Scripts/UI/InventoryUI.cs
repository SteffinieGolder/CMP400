using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script which controls the inventory UI. 
//Code adapted from this series by Jacquelynne Hei: https://www.youtube.com/watch?v=ZPYrdKMDsGI&list=PL4PNgDjMajPN51E5WzEi7cXzJ16BCHZXl&ab_channel=GameDevwithJacquelynneHei

public class InventoryUI : MonoBehaviour
{
    //Inventory name.
    public string inventoryName;
    //Character who owns the inventory.
    public GameObject owningCharacter;

    //List of UI slots in inventory. 
    public List<SlotsUI> slots = new List<SlotsUI>();

    //Inventory canvas.
    [SerializeField] private Canvas canvas;

    //Inventory object.
    private Inventory inventory;
    //Player script attached to the owning character.
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
            //Remove the grading image attached to this item if it was in a storage inventory.
            if (inventory.inventoryName == "Storage_C1" || inventory.inventoryName == "Storage_C2")
            {
                slots[UIManager.draggedSlot.slotID].ratingImage.SetActive(false);
            }

            //Check if user selected to drag one item from the slot.
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

        //Reset the dragged slot.
        UIManager.draggedSlot = null;
    }

    //Function which defines behaviour for if an item has been dragged from a slot.
    //Creates a smaller version of the item attached to the mouse pointer so it can be moved by the user.
    public void SlotBeginDrag(SlotsUI slot)
    {
        UIManager.draggedSlot = slot;
        UIManager.draggedIcon = Instantiate(UIManager.draggedSlot.itemIcon);
        UIManager.draggedIcon.transform.SetParent(canvas.transform);
        UIManager.draggedIcon.raycastTarget = false;
        UIManager.draggedIcon.rectTransform.sizeDelta = new Vector2(50, 50);
        MoveToMousePosition(UIManager.draggedIcon.gameObject);
    }

    //Moves the smaller spawned item wherever the mouse goes.
    public void SlotDrag()
    {
        MoveToMousePosition(UIManager.draggedIcon.gameObject);
    }

    //Destroys the smaller spawned item once the user has let go of the mouse button.
    public void SlotEndDrag()
    {
        Destroy(UIManager.draggedIcon.gameObject);
        UIManager.draggedIcon = null;
    }

    //Moves the item to the new inventory slot selected by the user.
    public void SlotDrop(SlotsUI slot)
    {
        if (slot)
        {
            if (UIManager.draggedSlot)
            {
                //Checks if the slot is in a storage inventory.
                if (slot.inventory.inventoryName == "Storage_C1" || slot.inventory.inventoryName == "Storage_C2")
                {
                    //Grades the item placed in the storage slot depending on the item and active character.
                    Item itemToGrade = GameManager.instance.itemManager.GetItemByName(UIManager.draggedSlot.inventory.slots[UIManager.draggedSlot.slotID].itemName);

                    if (itemToGrade)
                    {
                        if (itemToGrade.data.itemName == "Fish" || itemToGrade.data.itemName == "Strawberry" || itemToGrade.data.itemName == "Tomato" || itemToGrade.data.itemName == "Carrot")
                        {
                            if (GameManager.instance.characterManager.char1IsActive)
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

                //Moves a single item if the user has selected to move a single item.
                if (UIManager.dragSingle)
                {
                    //Move the item to the desired slot.
                    UIManager.draggedSlot.inventory.MoveSlot(UIManager.draggedSlot.slotID, slot.slotID, slot.inventory);

                    //If an item has been moved from a storage inventory, then remove the grading image. 
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
                    //Move the item to the desired slot.
                    UIManager.draggedSlot.inventory.MoveSlot(UIManager.draggedSlot.slotID, slot.slotID, slot.inventory,
                        UIManager.draggedSlot.inventory.slots[UIManager.draggedSlot.slotID].count);

                    //If an item has been moved from a storage inventory, then remove the grading image. 
                    if (UIManager.draggedSlot.inventory.inventoryName == "Storage_C1" || UIManager.draggedSlot.inventory.inventoryName == "Storage_C2")
                    {
                        UIManager.draggedSlot.ratingImage.SetActive(false);
                    }
                }

                //Refresh the inventory and reset the dragged slot.
                GameManager.instance.uiManager.RefreshAll();
                UIManager.draggedSlot = null;
            }
        }
    }

    //Moves the spawned item to the mouse position (so it can be dragged by the user).
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

    //Instantiate the slots in the inventory.
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
}
