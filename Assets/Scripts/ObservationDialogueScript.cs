using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservationDialogueScript : MonoBehaviour
{
    [SerializeField] int dialogueIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<Player>())
        {
            CharacterData charData = collision.GetComponent<Player>().charData;

            //Show Dialogue lines depending on current task number. 
            GameManager.instance.uiManager.SetDialogueData(charData.dialogueGroups[dialogueIndex].dialogueLines,
                charData.dialogueGroups[dialogueIndex].expressionTypes);

            Destroy(this);
        }
    }
}
