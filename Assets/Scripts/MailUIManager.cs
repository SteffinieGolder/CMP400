using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Script which controls the Mail UI.

public class MailUIManager : MonoBehaviour
{
    //Mail UI elements.
    [SerializeField] GameObject letterPanel;
    [SerializeField] TextMeshProUGUI letterText;
    [SerializeField] Sprite openMailSprite;
    GameObject mailObj;
    TextMeshProUGUI mailText;
    Image mailIcon;
    Image urgentIcon;
    //Dialogue index for mail content.
    int dialogueIndex;

    //Sets mail UI data to the object passed in and displays to the user.
    public void DisplayMailText(GameObject mail)
    {
        mailObj = mail;
        mailText = mail.GetComponentsInChildren<TextMeshProUGUI>(true)[0];
        mailIcon = mail.GetComponentsInChildren<Image>(true)[0];
        urgentIcon = mail.GetComponentsInChildren<Image>(true)[1];
        dialogueIndex = int.Parse(mail.GetComponentsInChildren<TextMeshProUGUI>(true)[1].text);
        letterText.text = mailText.text;
        letterPanel.SetActive(true);

        if (dialogueIndex != 0)
        {
            CharacterData charData = GameManager.instance.characterManager.activePlayer.charData;

            //Show Dialogue lines.
            GameManager.instance.uiManager.SetDialogueData(charData.GetDialogueGroup(dialogueIndex).dialogueLines,
                charData.GetDialogueGroup(dialogueIndex).expressionTypes);
        }
    }

    //Close the Mail UI element when the user is finished reading.
    public void CloseLetterUI()
    {
        letterPanel.SetActive(false);
        mailIcon.sprite = openMailSprite;
        urgentIcon.gameObject.SetActive(false);

        //Destory the trigger so it can't be read again.
        Destroy(mailObj.GetComponent<EventTrigger>());
    }

    //Function called when character refuses to read a letter.
    public void RefuseToRead(int dialogueIndex)
    {
        if (GameManager.instance.characterManager.char1IsActive)
        {
            CharacterData charData = GameManager.instance.characterManager.activePlayer.charData;

            //Show refusal dialogue lines.
            GameManager.instance.uiManager.SetDialogueData(charData.GetDialogueGroup(dialogueIndex).dialogueLines,
                charData.GetDialogueGroup(dialogueIndex).expressionTypes);
        }  
    }

}
