using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADHDEndTriggerScript : MonoBehaviour
{
    private bool firstInteractionComplete = false;
    private bool secondInteractionComplete = false;

    [SerializeField] int checkSellBoxTrueDialogueIndex = 0;
    [SerializeField] int checkSellBoxFalseDialogueIndex = 1;
    [SerializeField] int missedChanceDialogueIndex = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            if (GameManager.instance.taskManager.isWeedingComplete)
            {
                if (!firstInteractionComplete)
                {
                    CharacterData ADHDCharData = GameManager.instance.characterManager.char1PlayerScript.charData;
                    CharacterData NTCharData = GameManager.instance.characterManager.char2PlayerScript.charData;

                    //Show Dialogue lines.
                    GameManager.instance.uiManager.SetConversationDialogueData(ADHDCharData.axeBorrowConversationDialogue, NTCharData.axeBorrowConversationDialogue,
                        ADHDCharData, NTCharData);

                    firstInteractionComplete = true;
                }

                else if (GameManager.instance.uiManager.canTriggerSecondNTDialogue)
                {
                    if (!secondInteractionComplete)
                    {
                        //Have character despawn.
                        GameManager.instance.characterManager.activePlayer.charData.DisplayEndSeqSoloDialogue(missedChanceDialogueIndex);
                        secondInteractionComplete = true;
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            if (firstInteractionComplete && !secondInteractionComplete && !GameManager.instance.uiManager.startCheckingForStorageClosed)
            {
                if (GameManager.instance.uiManager.hasPlayerOpenedStorageInPlaythrough)
                {
                    GameManager.instance.characterManager.activePlayer.charData.DisplayEndSeqSoloDialogue(checkSellBoxTrueDialogueIndex);
                }

                else
                {
                    GameManager.instance.characterManager.activePlayer.charData.DisplayEndSeqSoloDialogue(checkSellBoxFalseDialogueIndex);
                }

                GameManager.instance.uiManager.startCheckingForStorageClosed = true;
            }
        }
    }
}
