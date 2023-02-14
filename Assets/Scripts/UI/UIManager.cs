using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Dictionary<string, InventoryUI> inventoryUIByName = new Dictionary<string, InventoryUI>();

    public GameObject inventoryPanel;

    public List<InventoryUI> inventoryUIs;

    public static SlotsUI draggedSlot;
    public static Image draggedIcon;
    public static bool dragSingle;

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
        if (inventoryPanel != null)
        {
            if (inventoryPanel.activeSelf == false)
            {
                inventoryPanel.SetActive(true);
                RefreshInventoryUI("Backpack");
            }
            else
            {
                inventoryPanel.SetActive(false);
            }
        }
    }

    public void RefreshInventoryUI(string inventoryName)
    {
        if(inventoryUIByName.ContainsKey(inventoryName))
        {
            inventoryUIByName[inventoryName].Refresh();
        }
    }

    public void RefreshAll()
    {
        foreach(KeyValuePair<string, InventoryUI> keyValuePair in inventoryUIByName)
        {
            keyValuePair.Value.Refresh();
        }
    }

    public InventoryUI GetInventoryUI(string inventoryName)
    {
        if(inventoryUIByName.ContainsKey(inventoryName))
        {
            return inventoryUIByName[inventoryName];
        }

        return null;
    }

    void Initialise()
    {
        foreach(InventoryUI ui in inventoryUIs)
        {
            if(!inventoryUIByName.ContainsKey(ui.inventoryName))
            {
                inventoryUIByName.Add(ui.inventoryName, ui);
            }
        }
    }
}
