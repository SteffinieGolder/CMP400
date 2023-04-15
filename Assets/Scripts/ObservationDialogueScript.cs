using UnityEngine;

//Script which triggers observational dialogue for the character.

public class ObservationDialogueScript : MonoBehaviour
{
    //Dialogue index.
    [SerializeField] int dialogueIndex;

    //Triggered if player enters the trigger.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Checks if the active character is the NT character.
        if (!GameManager.instance.characterManager.char1IsActive)
        {
            if (collision.transform.GetComponent<Player>())
            {
                CharacterData charData = collision.GetComponent<Player>().charData;

                //Show Dialogue lines depending on current task number. 
                GameManager.instance.uiManager.SetDialogueData(charData.dialogueGroups[dialogueIndex].dialogueLines,
                    charData.dialogueGroups[dialogueIndex].expressionTypes);

                //Destroy the trigger object after use.
                Destroy(this);
            }
        }
    }
}
