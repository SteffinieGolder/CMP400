using UnityEngine;

[CreateAssetMenu(menuName = "Tool Behaviour/Milk")]

public class MilkBehaviour : ToolBehaviour
{
    ItemData itemData;
    RaycastHit2D hit;
    CharacterData charData;

    public override bool CheckUseConditions(Vector2 position, ItemData item)
    {
        itemData = item;

        hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(position));

        if (hit)
        {
            if (hit.transform.GetComponent<Player>())
            {
                if (hit.transform.GetComponent<Player>().isActiveAndEnabled)
                {
                    charData = GameManager.instance.characterManager.activePlayer.charData;
                    if (hit.transform.GetComponent<CharBehaviourBase>().GetEmoteAsString() == "Tired" || hit.transform.GetComponent<CharBehaviourBase>().GetEmoteAsString() == "Frustrated")
                    return true;
                }
            }
        }

        return false;
    }

    public override bool PerformBehaviour()
    {
        if (GameManager.instance.characterManager.char1IsActive)
        {
            GameManager.instance.uiManager.DisplaySkyPanel(true);
            GameManager.instance.uiManager.SetDialogueData(charData.GetDialogueGroup(itemData.ADHDDialogueGroupIndexes[0]).dialogueLines,
                charData.GetDialogueGroup(itemData.ADHDDialogueGroupIndexes[0]).expressionTypes);

            GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().UpdateBehaviour(itemData.ADHDTimeValue, itemData.ADHDMultiplier, true);
        }

        else
        {
            //increase nt eneergy by amount
            GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().FullyRestoreEnergy();
        }

        //Return false if item is reusable
        return true;
    }

}
