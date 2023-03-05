using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script which stores and controls all inventory UIs accessed by the player. 

public class UIManager : MonoBehaviour
{
    public Dictionary<string, InventoryUI> inventoryUIByName = new Dictionary<string, InventoryUI>();

    public List<InventoryUI> inventoryUIs;
    public List<GameObject> backpackPanels;
    public List<GameObject> toolbarPanels;

    public static SlotsUI draggedSlot;
    public static Image draggedIcon;
    public static bool dragSingle;
    public static bool isPointerOnToggleUI;
    public static bool isPointerOnConstantUI;

    private void Awake()
    {
        Initialise();
    }

    private void Update()
    {
        //Toggle inventory on/off when player presses TAB key. 
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            dragSingle = true;
        }
        else
        {
            dragSingle = false;
        }
    }

    //Function which toggles inventory on/off by activating/deactivating UI panel element.
    public void ToggleInventory()
    {
        //THIS IS BAD////////////////////////////////////////////////////////////////
        if (backpackPanels != null)
        {
            if (GameManager.instance.characterManager.char1IsActive)
            {
                if (backpackPanels[0].activeSelf == false)
                {
                    backpackPanels[1].SetActive(false);
                    backpackPanels[0].SetActive(true);
                    RefreshInventoryUI("Backpack_C1");
                    Time.timeScale = 0;
                }
                else
                {
                    backpackPanels[0].SetActive(false);
                    SetPointerOnToggleUI(false);
                    Time.timeScale = 1;
                }
            }

            else
            {
                if (backpackPanels[1].activeSelf == false)
                {
                    backpackPanels[0].SetActive(false);
                    backpackPanels[1].SetActive(true);
                    RefreshInventoryUI("Backpack_C2");
                    Time.timeScale = 0;
                }
                else
                {
                    backpackPanels[1].SetActive(false);
                    SetPointerOnToggleUI(false);
                    Time.timeScale = 1;
                }
            }
        }
    }

    public void SwitchToolbar(bool isCharOne)
    {
        if(isCharOne)
        {
            toolbarPanels[1].SetActive(false);
            toolbarPanels[0].SetActive(true);
            RefreshInventoryUI("Toolbar_C1");
        }

        else
        {
            toolbarPanels[0].SetActive(false);
            toolbarPanels[1].SetActive(true);
            RefreshInventoryUI("Toolbar_C2");
        }
    }

    public void RefreshInventoryUI(string inventoryName)
    {
        if (inventoryUIByName.ContainsKey(inventoryName))
        {
            inventoryUIByName[inventoryName].Refresh();
        }
    }

    public void RefreshAll()
    {
        foreach (KeyValuePair<string, InventoryUI> keyValuePair in inventoryUIByName)
        {
            keyValuePair.Value.Refresh();
        }
    }

    public InventoryUI GetInventoryUI(string inventoryName)
    {
        if (inventoryUIByName.ContainsKey(inventoryName))
        {
            return inventoryUIByName[inventoryName];
        }

        return null;
    }

    void Initialise()
    {   
        foreach(InventoryUI ui in inventoryUIs)
        {
            if (!inventoryUIByName.ContainsKey(ui.inventoryName))
            {
                inventoryUIByName.Add(ui.inventoryName, ui);
            }
        }
    }

    public void RemoveFromInv()
    {
        if (draggedSlot)
        {
            InventoryUI current = GetInventoryUI(draggedSlot.inventory.inventoryName);

            if (current)
            {
                current.RemoveItem();
            }
        }
    }

    public void SetPointerOnToggleUI(bool isOnUI)
    {
        isPointerOnToggleUI = isOnUI;
    }

    public void SetPointerOnConstantUI(bool isOnUI)
    {
        isPointerOnConstantUI = isOnUI;
    }

}
