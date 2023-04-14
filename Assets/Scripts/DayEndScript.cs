using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayEndScript : MonoBehaviour
{
    CharacterData charData;
    InventoryManager inventoryManager;
    [SerializeField] int endDialogueIndex;
    [SerializeField] GameObject EndUIButton;
    [SerializeField] int fishAmount;
    [SerializeField] int cropAmount;

    private void Start()
    {
        charData = this.GetComponent<Player>().charData;
        inventoryManager = this.GetComponent<InventoryManager>();
    }

    void Update()
    {
        if (GameManager.instance.taskManager.totalTaskCounter == -1)
        {
            //Show Dialogue at desired index. This will be the NT character saying the day is finished. 
            GameManager.instance.uiManager.SetDialogueData(charData.GetDialogueGroup(endDialogueIndex).dialogueLines,
                charData.GetDialogueGroup(endDialogueIndex).expressionTypes);

            GameManager.instance.taskManager.totalTaskCounter = -2;
        }

        if (GameManager.instance.taskManager.totalTaskCounter == -2)
        {
            if (inventoryManager.DoesStorageContainEndItems(GameManager.instance.itemManager.GetItemByName("Fish")) == fishAmount)
            {
                if (inventoryManager.DoesStorageContainEndItems(GameManager.instance.itemManager.GetItemByName("Strawberry")) +
                    inventoryManager.DoesStorageContainEndItems(GameManager.instance.itemManager.GetItemByName("Carrot")) +
                    inventoryManager.DoesStorageContainEndItems(GameManager.instance.itemManager.GetItemByName("Tomato")) == cropAmount)
                {
                    //Reveal the end game button.
                    EndUIButton.SetActive(true);
                }
            }
        }
    }
}
