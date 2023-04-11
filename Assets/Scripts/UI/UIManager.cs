using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script which stores and controls all inventory UIs accessed by the player. 

public class UIManager : MonoBehaviour
{
    public Dictionary<string, InventoryUI> inventoryUIByName = new Dictionary<string, InventoryUI>();

    public GameObject dialoguePanel;
    public Image dialogueSprite;
    public TextMeshProUGUI dialogueTextUI;
    public GameObject dialoguePausePanel;

    public GameObject fadePanel;
    public GameObject skyPanel;

    public List<InventoryUI> inventoryUIs;
    public List<GameObject> backpackPanels;
    public List<GameObject> toolbarPanels;
    public List<GameObject> storagePanels;
    public GameObject removePanel;
    public int toggleUIAmount = 2;

    public static SlotsUI draggedSlot;
    public static Image draggedIcon;
    public static bool dragSingle;
    public static bool isPointerOnToggleUI;
    public static bool isPointerOnConstantUI;
    public static bool isCharacterInStorageInteractRange;

    private int currentActiveToggleUICount;
    private List<string> dialogueToShow;
    private List<CharacterData.FaceType> faceTypes;
    private int currentDialogueIndex = 0;
    private bool shouldRemoveUI = false;

    public bool startCheckingForStorageClosed = false;
    public bool canTriggerSecondNTDialogue = false;
    public bool hasPlayerOpenedStorageInPlaythrough = false;
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
        //IS THIS JUST CONSTANTLY SETTING/////////////////////////
        if(currentActiveToggleUICount ==0)
        {
            Time.timeScale = 1;
        }

        else
        {
            Time.timeScale = 0;
        }

        //Make function??
        if(shouldRemoveUI)
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

        if (isCharacterInStorageInteractRange)
        {
            if(Input.GetKeyDown(KeyCode.I))
            {
                //Pull up storage screen
                ShowStorageScreen();
            }
        }

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

        if(draggedSlot)
        {
            if(!removePanel.activeSelf)
            {
                removePanel.SetActive(true);
            }
        }

        else
        {
            if(removePanel.activeSelf)
            {
                removePanel.SetActive(false);
            }
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

    public void SetDialogueData(List<string> dialoguelines, List<CharacterData.FaceType> charFaceTypes)
    {
        dialogueToShow = dialoguelines;
        faceTypes = charFaceTypes;

        //showDialogue = true;
        currentDialogueIndex = 0;
        ShowDialogueBox();

        //ShowDialogueBox();
    }

   /* public void SetSingleDialogueLine(string line, CharacterData.FaceType type)
    {
        lineToShow = line;
        faceToShow = type;

        isSingleLine = true;
        ShowSingleDialogue();
    }

    private void ShowSingleDialogue()
    {
        dialogueSprite.sprite = GameManager.instance.characterManager.activePlayer.charData.charFaceSprites[(int)faceToShow];
        dialogueTextUI.text = lineToShow;

        if (!dialoguePanel.activeSelf)
        {
            dialoguePanel.SetActive(true);
            // Time.timeScale = 0;
        }
    }*/

    public void ShowDialogueBox()
    {
        if (currentDialogueIndex < dialogueToShow.Count)
        {
            dialogueSprite.sprite = GameManager.instance.characterManager.activePlayer.charData.charFaceSprites[(int)faceTypes[currentDialogueIndex]];
            dialogueTextUI.text = dialogueToShow[currentDialogueIndex];

            if (!dialoguePanel.activeSelf)
            {
                dialoguePanel.SetActive(true);
                dialoguePausePanel.SetActive(true);
                currentActiveToggleUICount++;
            }

            currentDialogueIndex++;
        }

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

            if(skyPanel.activeSelf)
            {
                DisplaySkyPanel(false);
            }
        }
    }

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

    public void ShowStorageScreen()
    {
        if (GameManager.instance.characterManager.char1IsActive)
        {
            if (storagePanels[0].activeSelf == false)
            {
                storagePanels[1].SetActive(false);
                storagePanels[0].SetActive(true);
                RefreshInventoryUI("Storage_C1");
                currentActiveToggleUICount++;

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

                if (startCheckingForStorageClosed)
                {
                    canTriggerSecondNTDialogue = true;
                    GameManager.instance.characterManager.activePlayer.charData.DisplayEndSeqSoloDialogue(checkedSellBoxDialogueIndex);
                    startCheckingForStorageClosed = false;
                }
            }
        }

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

    public void RemoveFromInv(GameObject triggerObj)
    {
        if (draggedSlot)
        {
            InventoryUI current = GetInventoryUI(draggedSlot.inventory.inventoryName);

            if (current)
            {
                //FIX THIS///////
                //if (triggerObj.name == "CompostBin" && draggedSlot.itemIcon.name == "Weed")
                // /{
                //   Debug.Log("Found weed");
                //}

                //else
                // {
                current.RemoveItem();
                //  }
                // }
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

    public void SetCharInStorageRange(bool isInRange)
    {
        isCharacterInStorageInteractRange = isInRange;
    }

}
