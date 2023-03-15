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

    public GameObject fadePanel;

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
    private bool showDialogue = false;

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

        if (isCharacterInStorageInteractRange)
        {
            if(Input.GetKeyDown(KeyCode.I))
            {
                //Pull up storage screen
                ShowStorageScreen();
            }
        }

        if (showDialogue)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                currentDialogueIndex++;

                if (currentDialogueIndex == dialogueToShow.Count)
                {
                    Debug.Log("Reset");
                    dialoguePanel.SetActive(false);
                    dialogueTextUI.text = "";
                    currentDialogueIndex = 0;
                    showDialogue = false;
                    //Time.timeScale = 1;
                    return;
                }

                ShowDialogueBox(currentDialogueIndex);
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

        Debug.Log(dialogueToShow.Count);
        Debug.Log(faceTypes.Count);

        showDialogue = true;
        ShowDialogueBox(0);
    }

    private void ShowDialogueBox(int currentIndex)
    {
        Debug.Log(currentIndex);
        dialogueSprite.sprite = GameManager.instance.characterManager.activePlayer.charData.charFaceSprites[(int)faceTypes[currentIndex]];
        dialogueTextUI.text = dialogueToShow[currentIndex];

        if (!dialoguePanel.activeSelf)
        {
            dialoguePanel.SetActive(true);
           // Time.timeScale = 0;
        }
    }

    public void FadeInOrOut(bool fadeOut)
    {
        Animation clip = fadePanel.GetComponent<Animation>();

        if(fadeOut)
        {
            clip.Play("FadeClip");
            
            Time.timeScale = 0;
        }

        else
        {
            clip.Play("FadeClip 1");
            Time.timeScale = 1;
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
            }
            else
            {
                storagePanels[0].SetActive(false);
                SetPointerOnToggleUI(false);
                currentActiveToggleUICount--;
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


    /// <summary>
    /// /MESSES UP DRAGGED SLOT IF ITS DRAGGED THEN PLAYER MOVES OUT OF RANGE BEFORE DROP
    /// </summary>
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
