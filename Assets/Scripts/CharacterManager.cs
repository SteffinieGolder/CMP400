using UnityEngine;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] GameObject char1;
    [SerializeField] GameObject char2;
    [SerializeField] GameObject char1Gate;
    [SerializeField] GameObject char2Gate;
    [SerializeField] int ADHDInitialDialogueIndex;
    [SerializeField] int NTInitialDialogueIndex;
    [SerializeField] GameObject endButton;

    public Player char1PlayerScript;
    public Player char2PlayerScript;
    private CharMovement char1MovementScript;
    private CharMovement char2MovementScript;
    private CharBehaviourBase char1BehaviourScript;
    private CharBehaviourBase char2BehaviourScript;

    private CamFollow camFollowScript;
    private ItemManager itemManager;

    public bool char1IsActive;
    public Player activePlayer;

    void Start()
    {
        itemManager = GameManager.instance.itemManager;

        char1PlayerScript = char1.GetComponent<Player>();
        char1MovementScript = char1.GetComponent<CharMovement>(); 
        char1BehaviourScript = char1.GetComponent<CharBehaviourBase>();

        char2PlayerScript = char2.GetComponent<Player>();
        char2MovementScript = char2.GetComponent<CharMovement>();
        char2BehaviourScript = char2.GetComponent<CharBehaviourBase>();

        camFollowScript = Camera.main.GetComponent<CamFollow>();

        InitialiseCharacter1InventoryItems();
        InitialiseCharacter2InventoryItems();

        SetChar2Active();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(char1IsActive)
            {
                SetChar2Active();
            }

            else
            {
                SetChar1Active();
            }
        }
    }

    void SetChar1Active()
    { 
        GameManager.instance.respawnManager.RespawnObject();
        GameManager.instance.respawnManager.RespawnTiles();
        GameManager.instance.taskManager.ResetCounters();
        GameManager.instance.dayAndNightManager.SetTime(36000);
        GameManager.instance.itemManager.ResetEquippedItem();
        GameManager.instance.uiManager.RemoveAllActiveUI();


        char1IsActive = true; 
        char2PlayerScript.enabled = false;
        char2MovementScript.enabled = false;
        char2BehaviourScript.enabled = false;
        char1PlayerScript.enabled = true;
        char1MovementScript.enabled = true;
        char1BehaviourScript.enabled = true;
        char1BehaviourScript.ResetEnergyBar();
        GameManager.instance.uiManager.SwitchToolbar(char1IsActive);
        char1Gate.SetActive(false);
        char2Gate.SetActive(true);
        activePlayer = char1PlayerScript;

        //Show character's initial dialogue lines.
        GameManager.instance.uiManager.SetDialogueData(char1PlayerScript.charData.GetDialogueGroup(ADHDInitialDialogueIndex).dialogueLines,
            char1PlayerScript.charData.GetDialogueGroup(ADHDInitialDialogueIndex).expressionTypes);
        camFollowScript.followTransform = char1.transform;
    }

    void SetChar2Active()
    {
        char1IsActive = false;
        char1PlayerScript.enabled = false;
        char1MovementScript.enabled = false;
        char1BehaviourScript.enabled = false;
        char2PlayerScript.enabled = true;
        char2MovementScript.enabled = true;
        char2BehaviourScript.enabled = true;
        char2BehaviourScript.ResetEnergyBar();
        camFollowScript.followTransform = char2.transform;
        GameManager.instance.uiManager.SwitchToolbar(char1IsActive);
        char1Gate.SetActive(true);
        char2Gate.SetActive(false);
        activePlayer = char2PlayerScript;

        //Show character's initial dialogue lines.
        GameManager.instance.uiManager.SetDialogueData(char2PlayerScript.charData.GetDialogueGroup(NTInitialDialogueIndex).dialogueLines,
            char2PlayerScript.charData.GetDialogueGroup(NTInitialDialogueIndex).expressionTypes);
    }

    void InitialiseCharacter1InventoryItems()
    {
        Dictionary<Item, int> char1StartItems = new Dictionary<Item, int>();
        char1StartItems.Add(itemManager.GetItemByName("Carrot Seeds"), 40);
        char1StartItems.Add(itemManager.GetItemByName("FishingRod"), 1);
        char1StartItems.Add(itemManager.GetItemByName("Sword"), 1);
        char1StartItems.Add(itemManager.GetItemByName("Bag"), 1);
        char1StartItems.Add(itemManager.GetItemByName("WateringCan"), 1);
        char1StartItems.Add(itemManager.GetItemByName("Milk"), 1);

        char1PlayerScript.inventoryManager.InitialiseInventoryWithItems(char1StartItems, char1PlayerScript.inventoryManager.backpack.inventoryName);
        GameManager.instance.uiManager.RefreshAll();
    }

    void InitialiseCharacter2InventoryItems()
    {
        Dictionary<Item, int> char2StartItems = new Dictionary<Item, int>();
        char2StartItems.Add(itemManager.GetItemByName("Carrot Seeds"), 40);
        char2StartItems.Add(itemManager.GetItemByName("FishingRod"), 1);
        char2StartItems.Add(itemManager.GetItemByName("Sword"), 1);
        char2StartItems.Add(itemManager.GetItemByName("Bag"), 1);
        char2StartItems.Add(itemManager.GetItemByName("WateringCan"), 1);
        char2StartItems.Add(itemManager.GetItemByName("Axe"), 1);
        char2StartItems.Add(itemManager.GetItemByName("Hoe"), 1);
        char2StartItems.Add(itemManager.GetItemByName("Milk"), 1);

        char2PlayerScript.inventoryManager.InitialiseInventoryWithItems(char2StartItems, char2PlayerScript.inventoryManager.backpack.inventoryName);

        Dictionary<Item, int> char2StorageItems = new Dictionary<Item, int>();
        char2StorageItems.Add(itemManager.GetItemByName("Carrot"), 20);
        char2StorageItems.Add(itemManager.GetItemByName("Strawberry"), 9);
        char2StorageItems.Add(itemManager.GetItemByName("Tomato"), 11);

        char2PlayerScript.inventoryManager.InitialiseInventoryWithItems(char2StorageItems, char2PlayerScript.inventoryManager.storage.inventoryName);

        GameManager.instance.uiManager.RefreshAll();
    }

    public void StartDayTwo()
    {
        SetChar1Active();
        endButton.SetActive(false);
    }
}
