using System.Collections.Generic;
using UnityEngine;

//Script which controls the toolbar UI element and adds highlight to selected toolbar slot. 
//Code adapted from this series by Jacquelynne Hei: https://www.youtube.com/watch?v=ZPYrdKMDsGI&list=PL4PNgDjMajPN51E5WzEi7cXzJ16BCHZXl&ab_channel=GameDevwithJacquelynneHei

public class ToolbarUI : MonoBehaviour
{
    //List of slots on toolbar. 
    [SerializeField] private List<SlotsUI> toolbarSlots = new List<SlotsUI>();
     
    //The slot selected by the player.
    private SlotsUI selectedSlot;

    private void Update()
    {
        //Check which key has been pressed (corresponds to slot on toolbar UI).
        CheckAlphaNumericKeys();
    }

    //Function which changes highlighted/selected slot. 
    public void SelectSlot(int index)
    {
        if (toolbarSlots.Count == 9)
        {
            //Sets previous selected slot highlight to false (removes highlight from previous) if previous exists.
            if (selectedSlot != null)
            {
                selectedSlot.SetHighlight(false);
            }

            //Apply highlight to new selected slot. 
            selectedSlot = toolbarSlots[index];
            selectedSlot.SetHighlight(true);

            //Equips the item at the selected toolbar slot. 
            GameManager.instance.itemManager.EquipItem(selectedSlot.inventory.slots[selectedSlot.slotID].itemName, selectedSlot);
        }
    }

    //Function which checks player input and selects a slot depending on which number was pressed. 
    private void CheckAlphaNumericKeys()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectSlot(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectSlot(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectSlot(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectSlot(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectSlot(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectSlot(5);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelectSlot(6);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SelectSlot(7);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SelectSlot(8);
        }
    }
}
