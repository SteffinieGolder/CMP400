using UnityEngine;
using System.Collections.Generic;

//Script which controls the switch between characters in the game.

public class CharacterManager : MonoBehaviour
{
    //Character objects.
    [SerializeField] GameObject char1;
    [SerializeField] GameObject char2;
    //Character farm gates.
    [SerializeField] GameObject char1Gate;
    [SerializeField] GameObject char2Gate;
    //Dialogue indexes.
    [SerializeField] int ADHDInitialDialogueIndex;
    [SerializeField] int NTInitialDialogueIndex;
    //UI element to end the game.
    [SerializeField] GameObject endButton;
    //Transforms used for positioning characters in conversation.
    [SerializeField] GameObject ADHDConvoTransform;
    [SerializeField] GameObject NTConvoTransform;

    //Character scripts.
    public Player char1PlayerScript;
    public Player char2PlayerScript;
    private CharMovement char1MovementScript;
    private CharMovement char2MovementScript;
    private CharBehaviourBase char1BehaviourScript;
    private CharBehaviourBase char2BehaviourScript;

    //Camera script.
    private CamFollow camFollowScript;
    //Item manager.
    private ItemManager itemManager;

    //Set if character 1 (ADHD) is active.
    public bool char1IsActive;
    public Player activePlayer;

    //Initial position of the NT character.
    private Vector3 char2InitialPosition;
    //Game over bool. Set when game is finished.
    public bool isGameOver = false;

    //Initialise.
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

        char2InitialPosition = char2.transform.position;

        SetChar2Active();
    }

    void Update()
    {
        //Testing.
        /*if(Input.GetKeyDown(KeyCode.Space))
        {
            if(char1IsActive)
            {
                SetChar2Active();
            }

            else
            {
                EndDay();
            }
        }*/

        //Check if character 1 is active and move the NT character to the right place depending on the stage the player is at in the game.
        //Need to move to the fence and back when in conversation/conversation is finished.
        if (char1IsActive)
        {
            if (GameManager.instance.taskManager.isWeedingComplete)
            {
                if (!GameManager.instance.uiManager.canTriggerSecondNTDialogue)
                {
                    if (char2.transform.position != NTConvoTransform.transform.position)
                    {
                        char2.transform.position = NTConvoTransform.transform.position;
                        char2.SetActive(true);
                    }
                }

                else
                {
                    if (char2.transform.position != char2InitialPosition)
                    {
                        char2.transform.position = char2InitialPosition;
                        char2.SetActive(false);
                    }
                }
            }
        }
    }

    //Set character 1 active.
    void SetChar1Active()
    { 
        //Respawn objects and tiles changed through NT gameplay.
        GameManager.instance.respawnManager.RespawnObject();
        GameManager.instance.respawnManager.RespawnTiles();
        GameManager.instance.taskManager.ResetCounters();
        //Reset time.
        GameManager.instance.dayAndNightManager.SetTime(36000);
        //Reset UI.
        GameManager.instance.itemManager.ResetEquippedItem();
        GameManager.instance.uiManager.RemoveAllActiveUI();

        //Switch characters.
        char1.SetActive(true);
        char1IsActive = true;
        char2BehaviourScript.currentEmote = char2BehaviourScript.charEmotes[0];
        char2.SetActive(false);
        char2.transform.position = char2InitialPosition;
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
        //Switch characters.
        GameManager.instance.uiManager.FadeInOrOut(true);
        char1IsActive = false;
        char2.SetActive(true);
        char1.SetActive(false);
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

    //Initialise character 1's inventory.
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

    //Initialise character 2's inventory.
    void InitialiseCharacter2InventoryItems()
    {
        Dictionary<Item, int> char2StartItems = new Dictionary<Item, int>();
        char2StartItems.Add(itemManager.GetItemByName("Hoe"), 1);
        char2StartItems.Add(itemManager.GetItemByName("Carrot Seeds"), 40);
        char2StartItems.Add(itemManager.GetItemByName("WateringCan"), 1);
        char2StartItems.Add(itemManager.GetItemByName("Sword"), 1);
        char2StartItems.Add(itemManager.GetItemByName("FishingRod"), 1);
        char2StartItems.Add(itemManager.GetItemByName("Axe"), 1);
        char2StartItems.Add(itemManager.GetItemByName("Milk"), 1);
        char2StartItems.Add(itemManager.GetItemByName("Bag"), 1);

        char2PlayerScript.inventoryManager.InitialiseInventoryWithItems(char2StartItems, char2PlayerScript.inventoryManager.backpack.inventoryName);

        Dictionary<Item, int> char2StorageItems = new Dictionary<Item, int>();
        char2StorageItems.Add(itemManager.GetItemByName("Carrot"), 20);
        char2StorageItems.Add(itemManager.GetItemByName("Strawberry"), 9);
        char2StorageItems.Add(itemManager.GetItemByName("Tomato"), 11);

        char2PlayerScript.inventoryManager.InitialiseInventoryWithItems(char2StorageItems, char2PlayerScript.inventoryManager.storage.inventoryName);

        GameManager.instance.uiManager.RefreshAll();
    }

    //Ends the current day depending on current active character. 
    public void EndDay()
    {
        if (char1IsActive)
        {
            //Fades out the game and displays final dialogue.
            GameManager.instance.uiManager.FadeInOrOut(true);
            GameManager.instance.characterManager.activePlayer.charData.DisplayEndSeqSoloDialogue(3);
            isGameOver = true;
        }

        else
        {
            //Starts day 2.
            endButton.SetActive(false);
            GameManager.instance.uiManager.FadeInOrOut(true);
            SetChar1Active();
        }
    }
}
