using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Script which controls the slots UI. 

public class SlotsUI : MonoBehaviour
{
    public int slotID;
    public Inventory inventory;
    public GameObject ratingImage;

    //Item icon and text which displays number held. 
    public Image itemIcon;
    public TextMeshProUGUI quantityText;

    //Highlight object for toolbar slots.
    [SerializeField] private GameObject highlightObj;

    //Function which adds item information to inventory slot.
    public void SetItem(Inventory.Slot slot)
    {
        if(slot !=null)
        {
            itemIcon.sprite = slot.icon;
            itemIcon.color = new Color(1, 1, 1);
            quantityText.text = slot.count.ToString();
        }
    }

    //Function which removes item information from inventory slot. 
    public void SetEmpty()
    {
        itemIcon.sprite = null;
        itemIcon.color = new Color(1, 1, 1, 0);
        quantityText.text = "";
    }

    //Function which toggles slot highlight. 
    public void SetHighlight(bool isOn)
    {
        highlightObj.SetActive(isOn);
    }

}
