using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MailUIManager : MonoBehaviour
{
    [SerializeField] GameObject letterPanel;
    [SerializeField] TextMeshProUGUI letterText;
    [SerializeField] Sprite openMailSprite;

    GameObject mailObj;
    TextMeshProUGUI mailText;
    Image mailIcon;
    Image urgentIcon;

    public void DisplayMailText(GameObject mail)
    {
        //Check character
        //Check if its important mail
        //check if character will read it
        mailObj = mail;
        mailText = mail.GetComponentInChildren<TextMeshProUGUI>(true);
        mailIcon = mail.GetComponentsInChildren<Image>(true)[0];
        urgentIcon = mail.GetComponentsInChildren<Image>(true)[1];
        letterText.text = mailText.text;
        letterPanel.SetActive(true);

    }

    public void CloseLetterUI()
    {
        letterPanel.SetActive(false);
        mailIcon.sprite = openMailSprite;
        urgentIcon.gameObject.SetActive(false);
        Destroy(mailObj.GetComponent<EventTrigger>());
    }

    public void RefuseToRead(int dialogueIndex)
    {
        if (GameManager.instance.characterManager.char1IsActive)
        {
            CharacterData charData = GameManager.instance.characterManager.activePlayer.charData;

            //Show Dialogue lines.
            GameManager.instance.uiManager.SetDialogueData(charData.GetDialogueGroup(dialogueIndex).dialogueLines,
                charData.GetDialogueGroup(dialogueIndex).expressionTypes);
        }  
    }

}
