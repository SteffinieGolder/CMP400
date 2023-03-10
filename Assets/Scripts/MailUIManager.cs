using System.Collections;
using System.Collections.Generic;
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
        mailObj = mail;
        mailText = mail.GetComponentInChildren<TextMeshProUGUI>(true);
        mailIcon = mail.GetComponentsInChildren<Image>()[0];
        urgentIcon = mail.GetComponentsInChildren<Image>()[1];

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

    public void RefuseToRead()
    {
        Debug.Log("eek");
    }

}
