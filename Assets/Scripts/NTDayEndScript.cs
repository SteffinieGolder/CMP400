using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTDayEndScript : MonoBehaviour
{
    CharacterData charData;
    [SerializeField] int endDialogueIndex;
    [SerializeField] GameObject EndUIButton;

    private void Start()
    {
        charData = this.GetComponent<Player>().charData;
    }

    void Update()
    {
        if (GameManager.instance.taskManager.totalTaskCounter == -1)
        {
            //Show Dialogue at desired index. This will be the NT character saying the day is finished. 
            GameManager.instance.uiManager.SetDialogueData(charData.GetDialogueGroup(endDialogueIndex).dialogueLines,
                charData.GetDialogueGroup(endDialogueIndex).expressionTypes);

            //Reveal the end game button.
            EndUIButton.SetActive(true);
            GameManager.instance.taskManager.totalTaskCounter = -2;
        }
    }
}
