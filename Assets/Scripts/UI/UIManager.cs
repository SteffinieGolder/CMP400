using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Script which stores and controls all inventory UIs accessed by the player. 
//Code adapted from this series by Jacquelynne Hei: https://www.youtube.com/watch?v=ZPYrdKMDsGI&list=PL4PNgDjMajPN51E5WzEi7cXzJ16BCHZXl&ab_channel=GameDevwithJacquelynneHei

public class UIManager : MonoBehaviour
{
    //Dictionary of all inventory UI elements in game.
    public Dictionary<string, InventoryUI> inventoryUIByName = new Dictionary<string, InventoryUI>();

    //Dialogue UI panel.
    public GameObject dialoguePanel;
    //Dialogue sprite object.
    public Image dialogueSprite;
    //Dialogue text element.
    public TextMeshProUGUI dialogueTextUI;
    //Panel which prevents user from touching other UI elements when dialogue is active. 
    public GameObject dialoguePausePanel;

    //UI element for the screen fade out.
    public GameObject fadePanel;
    //UI element for the ADHD sky fade out. 
    public GameObject skyPanel;

    //UI elements.
    public List<InventoryUI> inventoryUIs;
    public List<GameObject> backpackPanels;
    public List<GameObject> toolbarPanels;
    public List<GameObject> storagePanels;
    public GameObject removePanel;
    //The amount of 'toggle' UI - the inventories which can be toggled on and off by the user. 
    public int toggleUIAmount = 2;

    //UI variables for the dragged slot (if user decides to move an item around in their inventory).
    public static SlotsUI draggedSlot;
    public static Image draggedIcon;
    public static bool dragSingle;
    //Variables which track where the mouse pointer is.
    public static bool isPointerOnToggleUI;
    public static bool isPointerOnConstantUI;
    //Used to determine if player can open storage box.
    public static bool isCharacterInStorageInteractRange;

    //Keeps track of current active toggle inventories in game. 
    private int currentActiveToggleUICount;

    //Dialogue lines to show.
    private List<string> dialogueToShow;
    //Dialogue faces to show.
    private List<CharacterData.FaceType> faceTypes;
    //Conversation dialogue for the first speaker.
    private List<CharacterData.DialogueGroup> conversationGroup1stSpeaker;
    //Conversation dialogue for the second speaker.
    private List<CharacterData.DialogueGroup> conversationGroup2ndSpeaker;
    //Used to tell script if incoming dialogue is a conversation or single person. 
    private bool isConversation = false;
    //Character data for different speakers in conversation.
    CharacterData firstSpeakerData;
    CharacterData secondSpeakerData;
    //Keep track of index in dialogue.
    private int currentDialogueIndex = 0;
    //Keep track of index for first speaker in conversation.
    private int speaker1GroupIndex = 0;
    //Keep track of index for second speaker in conversation.
    private int speaker2GroupIndex = 0;
    //Tells script to remove UI element.
    private bool shouldRemoveUI = false;

    //Triggered once ADHD day is about to end and they should check their storage box.
    public bool startCheckingForStorageClosed = false;
    //Triggered once ADHD day is about to end and they should return to the other character.
    public bool canTriggerSecondNTDialogue = false;
    //Triggered if player has been in the storage box yet in the playthrough, and changes dialogue accordingly. 
    public bool hasPlayerOpenedStorageInPlaythrough = false;
    //Dialogue index for the dialogue line which tells the user to check the storage box. 
    [SerializeField] int checkedSellBoxDialogueIndex = 2;

    private void Awake()
    {
        Initialise();
        currentActiveToggleUICount = 0;
        dialogueToShow = new List<string>();
        faceTypes = new List<CharacterData.FaceType>();
    }

    private void Update()
    {
        //Set timescale (pause or unpause) if a toggle UI is open/closed.
        if (currentActiveToggleUICount == 0)
        {
            Time.timeScale = 1;
        }

        else
        {
            Time.timeScale = 0;
        }

        //Removes UI elements if they're active and this is triggered.
        if (shouldRemoveUI)
        {
            if (fadePanel.activeSelf)
            {
                Animator animator = fadePanel.GetComponent<Animator>();
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    fadePanel.SetActive(false);
                    shouldRemoveUI = false;
                }
            }

            else if (skyPanel.activeSelf)
            {
                Animator animator = skyPanel.GetComponent<Animator>();
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    skyPanel.SetActive(false);
                    shouldRemoveUI = false;
                }
            }
        }

        //Allows the user to press I to open their storage UI if they are in range.
        if (isCharacterInStorageInteractRange)
        {
            if(Input.GetKeyDown(KeyCode.I))
            {
                //Pull up storage screen.
                ShowStorageScreen();
            }
        }

        //Toggle inventory on/off when player presses TAB key. 
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }

        //User can hold shift to drag one item around in inventories rather than full stack. 
        if (Input.GetKey(KeyCode.LeftShift))
        {
            dragSingle = true;
        }
        else
        {
            dragSingle = false;
        }

        //Activate this panel to prevent user from dragging an item into the world.
        if (draggedSlot)
        {
            if (!removePanel.activeSelf)
            {
                removePanel.SetActive(true);
            }
        }

        else
        {
            if (removePanel.activeSelf)
            {
                removePanel.SetActive(false);
            }
        }
    }

    //Removes all active UI.
    public void RemoveAllActiveUI()
    {
        if (backpackPanels[1].activeSelf)
        {
            ToggleInventory();
        }

        if (storagePanels[1].activeSelf)
        {
            ShowStorageScreen();
        }

    }

    //Function which toggles inventory on/off by activating/deactivating UI panel element.
    public void ToggleInventory()
    {
        if (backpackPanels != null)
        {
            if (GameManager.instance.characterManager.char1IsActive)
            {
                if (backpackPanels[0].activeSelf == false)
                {
                    backpackPanels[1].SetActive(false);
                    backpackPanels[0].SetActive(true);
                    RefreshInventoryUI("Backpack_C1");
                    currentActiveToggleUICount++;
                }
                else
                {
                    backpackPanels[0].SetActive(false);
                    SetPointerOnToggleUI(false);
                    currentActiveToggleUICount--;
                }
            }

            else
            {
                if (backpackPanels[1].activeSelf == false)
                {
                    backpackPanels[0].SetActive(false);
                    backpackPanels[1].SetActive(true);
                    RefreshInventoryUI("Backpack_C2");
                    currentActiveToggleUICount++;
                }
                else
                {
                    backpackPanels[1].SetActive(false);
                    SetPointerOnToggleUI(false);
                    currentActiveToggleUICount--;
                }
            }
        }
    }

    //Sets the dialogue data to show.
    public void SetDialogueData(List<string> dialoguelines, List<CharacterData.FaceType> charFaceTypes)
    {
        dialogueToShow = dialoguelines;
        faceTypes = charFaceTypes;

        currentDialogueIndex = 0;
        ShowDialogueBox();
    }

    //Sets dialogue data for a conversation.
    public void SetConversationDialogueData(List<CharacterData.DialogueGroup> firstSpeakerDialogueLines, List<CharacterData.DialogueGroup> secondSpeakerDialoguelines, 
        CharacterData firstSpeaker, CharacterData secondSpeaker)
    {
        conversationGroup1stSpeaker = firstSpeakerDialogueLines;
        conversationGroup2ndSpeaker = secondSpeakerDialoguelines;

        currentDialogueIndex = 0;
        speaker1GroupIndex = 0;
        speaker2GroupIndex = 0;
        isConversation = true;
        firstSpeakerData = firstSpeaker;
        secondSpeakerData = secondSpeaker;
        ShowDialogueBox();
    }
    
    //Show dialogue box to user depending on if its a conversation or not.
    public void ShowDialogueBox()
    {
        if (isConversation)
        {
            ShowConversationDialogue();
        }

        else
        {
            ShowSingleCharDialogue();
        }
      
    }

    //Shows single character dialogue.
    public void ShowSingleCharDialogue()
    {
        //Goes through each dialogue element and and displays to the user.
        if (currentDialogueIndex < dialogueToShow.Count)
        {
            dialogueSprite.sprite = GameManager.instance.characterManager.activePlayer.charData.charFaceSprites[(int)faceTypes[currentDialogueIndex]];
            dialogueTextUI.text = dialogueToShow[currentDialogueIndex];

            //Activate the UI panel.
            if (!dialoguePanel.activeSelf)
            {
                dialoguePanel.SetActive(true);
                dialoguePausePanel.SetActive(true);
                currentActiveToggleUICount++;
            }

            //Increment the index used for looping through the dialogue elements. 
            currentDialogueIndex++;
        }

        //Remove the dialogue UI elements once all of the lines have been displayed. 
        else
        {
            dialoguePanel.SetActive(false);
            dialoguePausePanel.SetActive(false);
            dialogueTextUI.text = "";
            currentDialogueIndex = 0;
            currentActiveToggleUICount--;

            if (fadePanel.activeSelf)
            {
                if (!GameManager.instance.characterManager.isGameOver)
                {
                    FadeInOrOut(false);
                }
            }

            if (skyPanel.activeSelf)
            {
                DisplaySkyPanel(false);
            }

            //Used for the ADHD ending. 
            if (GameManager.instance.taskManager.totalTaskCounter == 0)
            {
                GameManager.instance.taskManager.totalTaskCounter--;
            }
        }
    }

    //Show conversation dialogue to the user. 
    public void ShowConversationDialogue()
    {
        //Checks if last speaker still has lines to say.
        if (speaker2GroupIndex < conversationGroup1stSpeaker.Count)
        {
            //Display the 2nd speaker's dialogue lines if the first is a step ahead. 
            if (speaker1GroupIndex > speaker2GroupIndex)
            {
                if (currentDialogueIndex <= conversationGroup2ndSpeaker[speaker2GroupIndex].dialogueLines.Count)
                {
                    dialogueSprite.sprite = secondSpeakerData.charFaceSprites[(int)conversationGroup2ndSpeaker[speaker2GroupIndex].expressionTypes[currentDialogueIndex]];
                    dialogueTextUI.text = conversationGroup2ndSpeaker[speaker2GroupIndex].dialogueLines[currentDialogueIndex];

                    currentDialogueIndex++;
                }

                if (currentDialogueIndex == conversationGroup2ndSpeaker[speaker2GroupIndex].dialogueLines.Count)
                {
                    speaker2GroupIndex++;
                    currentDialogueIndex = 0;
                }
            }

            else
            {
                //Display the first speakers dialogue lines.
                if (currentDialogueIndex < conversationGroup1stSpeaker[speaker1GroupIndex].dialogueLines.Count)
                {
                    dialogueSprite.sprite = firstSpeakerData.charFaceSprites[(int)conversationGroup1stSpeaker[speaker1GroupIndex].expressionTypes[currentDialogueIndex]];
                    dialogueTextUI.text = conversationGroup1stSpeaker[speaker1GroupIndex].dialogueLines[currentDialogueIndex];

                    //Activate the UI panels.
                    if (!dialoguePanel.activeSelf)
                    {
                        dialoguePanel.SetActive(true);
                        dialoguePausePanel.SetActive(true);
                        currentActiveToggleUICount++;
                    }

                    currentDialogueIndex++;
                }

                if (currentDialogueIndex == conversationGroup1stSpeaker[speaker1GroupIndex].dialogueLines.Count)
                {
                    speaker1GroupIndex++;
                    currentDialogueIndex = 0;
                }
            }
        }

        //Remove the UI elements. 
        else
        {
            dialoguePanel.SetActive(false);
            dialoguePausePanel.SetActive(false);
            dialogueTextUI.text = "";
            currentDialogueIndex = 0;
            currentActiveToggleUICount--;

            if (fadePanel.activeSelf)
            {
                FadeInOrOut(false);
            }

            if (skyPanel.activeSelf)
            {
                DisplaySkyPanel(false);
            }

            isConversation = false;
        }
    }

    //Function which plays a fade in/out animation.
    public void FadeInOrOut(bool fadeOut)
    {
        Animator animator = fadePanel.GetComponent<Animator>();

        if (fadeOut)
        {
            fadePanel.SetActive(true);
            animator.SetTrigger("FadeIn");
        }

        else
        {
            animator.SetTrigger("FadeOut");
            shouldRemoveUI = true;
        }

    }

    //Displays the sky panel (used when ADHD character zones out).
    public void DisplaySkyPanel(bool shouldDisplayPanel)
    {
        Animator animator = skyPanel.GetComponent<Animator>();

        if (shouldDisplayPanel)
        {
            skyPanel.SetActive(true);
            animator.SetTrigger("FadeIn");
        }

        else
        {
            animator.SetTrigger("FadeOut");
            shouldRemoveUI = true;
        }
    }

    //Shows storage screen.
    public void ShowStorageScreen()
    {
        //Checks if ADHD character is active.
        if (GameManager.instance.characterManager.char1IsActive)
        {
            if (storagePanels[0].activeSelf == false)
            {
                storagePanels[1].SetActive(false);
                storagePanels[0].SetActive(true);
                RefreshInventoryUI("Storage_C1");
                currentActiveToggleUICount++;

                //Set this to true as player has opened the storage UI.
                if (!hasPlayerOpenedStorageInPlaythrough)
                {
                    hasPlayerOpenedStorageInPlaythrough = true;
                }
            }
            else
            {
                storagePanels[0].SetActive(false);
                SetPointerOnToggleUI(false);
                currentActiveToggleUICount--;

                //If the bool has been set to start checking if the user has checked the storage for their lost item, then display the dialogue.
                if (startCheckingForStorageClosed)
                {
                    canTriggerSecondNTDialogue = true;
                    GameManager.instance.characterManager.activePlayer.charData.DisplayEndSeqSoloDialogue(checkedSellBoxDialogueIndex);
                    startCheckingForStorageClosed = false;
                }
            }
        }

        //If NT character is active either open or close storage UI.
        else
        {
            if (storagePanels[1].activeSelf == false)
            {
                storagePanels[0].SetActive(false);
                storagePanels[1].SetActive(true);
                RefreshInventoryUI("Storage_C2");
                currentActiveToggleUICount++;
            }
            else
            {
                storagePanels[1].SetActive(false);
                SetPointerOnToggleUI(false);
                currentActiveToggleUICount--;
            }
        }
    }

    //Closes the storage UI.
    public void CloseStorageUI()
    {
        if (storagePanels != null)
        {
            if (GameManager.instance.characterManager.char1IsActive)
            {
                if (storagePanels[0].activeSelf)
                {
                    storagePanels[0].SetActive(false);           
                }

                if (startCheckingForStorageClosed)
                {
                    canTriggerSecondNTDialogue = true;
                    GameManager.instance.characterManager.activePlayer.charData.DisplayEndSeqSoloDialogue(checkedSellBoxDialogueIndex);
                    startCheckingForStorageClosed = false;
                }
            }

            else
            {
                if (storagePanels[1].activeSelf)
                {
                    storagePanels[1].SetActive(false);

                }
            }
        }
    }

    //Returns if character is in a conversation.
    public bool IsCharacterInConversation()
    {
        return isConversation;
    }

    //Switches the active toolbar depending on which character is active.
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

    //Refreshes inventory UI element. 
    public void RefreshInventoryUI(string inventoryName)
    {
        if (inventoryUIByName.ContainsKey(inventoryName))
        {
            inventoryUIByName[inventoryName].Refresh();
        }
    }

    //Refreshes all inventory UI elements. 
    public void RefreshAll()
    {
        foreach (KeyValuePair<string, InventoryUI> keyValuePair in inventoryUIByName)
        {
            keyValuePair.Value.Refresh();
        }
    }

    //Returns UI element.
    public InventoryUI GetInventoryUI(string inventoryName)
    {
        if (inventoryUIByName.ContainsKey(inventoryName))
        {
            return inventoryUIByName[inventoryName];
        }

        return null;
    }

    //Initialise inventory UI elements.
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

    //Remove object from inventory.
    public void RemoveFromInv(GameObject triggerObj)
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

    //Determines if pointer is on a toggle UI (backpack).
    public void SetPointerOnToggleUI(bool isOnUI)
    {
        isPointerOnToggleUI = isOnUI;
    }

    //Determines if pointer is on a constant UI (toolbar).
    public void SetPointerOnConstantUI(bool isOnUI)
    {
        isPointerOnConstantUI = isOnUI;
    }

    //Determines if player can interact with their storage box.
    public void SetCharInStorageRange(bool isInRange)
    {
        isCharacterInStorageInteractRange = isInRange;
    }

}
