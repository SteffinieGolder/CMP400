using UnityEngine;

//Script which controls the ADHD day ending sequence (from the point when they start looking for their axe).

public class ADHDEndTriggerScript : MonoBehaviour
{
    //Keeps track of the different interactions in the ending sequence.
    private bool firstInteractionComplete = false;
    private bool secondInteractionComplete = false;

    //Dialogue indexes.
    [SerializeField] int checkSellBoxTrueDialogueIndex = 0;
    [SerializeField] int checkSellBoxFalseDialogueIndex = 1;

    //Triggered when ADHD char enters the trigger.
    //Trigger located outside NT chars house.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Checks if collision is a player character.
        if (collision.gameObject.GetComponent<Player>())
        {
            //Checks if the weeding task is complete (this is the task before finding the axe).
            if (GameManager.instance.taskManager.isWeedingComplete)
            {
                //Checks if the first interaction has been complete or not. 
                if (!firstInteractionComplete)
                {
                    //Character data.
                    CharacterData ADHDCharData = GameManager.instance.characterManager.char1PlayerScript.charData;
                    CharacterData NTCharData = GameManager.instance.characterManager.char2PlayerScript.charData;

                    //Show Dialogue lines for the conversation between the NT and ADHD characters.
                    GameManager.instance.uiManager.SetConversationDialogueData(ADHDCharData.axeBorrowConversationDialogue, NTCharData.axeBorrowConversationDialogue,
                        ADHDCharData, NTCharData);

                    //Set the first interaction to complete, advance time and set energy levels.
                    firstInteractionComplete = true;
                    GameManager.instance.dayAndNightManager.AdvanceCurrentTime(3600);
                    GameManager.instance.characterManager.char1PlayerScript.GetComponent<CharBehaviourBase>().SetEnergyLevel(0.7f);
                }

                //Process the second interaction.
                else if (GameManager.instance.uiManager.canTriggerSecondNTDialogue)
                {
                    if (!secondInteractionComplete)
                    {
                        secondInteractionComplete = true;
                        GameManager.instance.taskManager.totalTaskCounter = -1;
                    }
                }
            }
        }
    }

    //Triggered when ADHD char exits the trigger.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            //Displays a dialogue line depending on if player has looked in their storage box yet. If they've checked it already the dialogue will say to check again.
            //If not, it will say to check it.
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
