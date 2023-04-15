using UnityEngine;

//Script which controls the ending of a day in game. Users can end the game once they've placed items in the storage box.

public class DayEndScript : MonoBehaviour
{
    CharacterData charData;
    InventoryManager inventoryManager;
    //Dialogue index.
    [SerializeField] int endDialogueIndex;
    [SerializeField] GameObject endUIButton;
    //Item amounts that should be in storage before the end can be triggered.
    [SerializeField] int fishAmount;
    [SerializeField] int cropAmount;

    private void Start()
    {
        charData = this.GetComponent<Player>().charData;
        inventoryManager = this.GetComponent<InventoryManager>();
    }

    void Update()
    {
        //Ensures this is only done once.
        if (GameManager.instance.taskManager.totalTaskCounter == -1)
        {
            //Show Dialogue at desired index. This will be the NT character saying the day is finished. 
            GameManager.instance.uiManager.SetDialogueData(charData.GetDialogueGroup(endDialogueIndex).dialogueLines,
                charData.GetDialogueGroup(endDialogueIndex).expressionTypes);

            GameManager.instance.taskManager.totalTaskCounter = -2;
        }

        //Ensures this is only done once.
        if (GameManager.instance.taskManager.totalTaskCounter == -2)
        {
            //Ensures the correct amount of items are in storage before the game can end.
            if (inventoryManager.DoesStorageContainEndItems(GameManager.instance.itemManager.GetItemByName("Fish")) == fishAmount)
            {
                if (inventoryManager.DoesStorageContainEndItems(GameManager.instance.itemManager.GetItemByName("Strawberry")) +
                    inventoryManager.DoesStorageContainEndItems(GameManager.instance.itemManager.GetItemByName("Carrot")) +
                    inventoryManager.DoesStorageContainEndItems(GameManager.instance.itemManager.GetItemByName("Tomato")) == cropAmount)
                {
                    //Reveal the end game button.
                    endUIButton.SetActive(true);
                }
            }
        }
    }
}
