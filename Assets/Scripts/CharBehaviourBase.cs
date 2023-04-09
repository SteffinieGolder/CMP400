using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class CharBehaviourBase : MonoBehaviour
{
    public Slider energyBarSlider;
    public List<EmoteData> charEmotes;
    public GameObject emoteObject;
    protected float energyCellSize = 0.126f;
    protected float currentEnergySliderNum = 1;
    protected float timeBetweenChanges = 8000;
    protected float currentTime;
    protected DayAndNightManager timeManager;
    public EmoteData currentEmote;
    private int rejectDialogueIndex;

    enum EmoteTypes
    {
        HAPPY = 0,
        TIRED = 1,
        FRUSTRATED = 2
    }

    public void Initialise()
    {
        currentEnergySliderNum = energyBarSlider.value;
        timeManager = GameManager.instance.dayAndNightManager;
        currentTime = timeManager.GetCurrentTime();

        currentEmote = charEmotes[(int)EmoteTypes.HAPPY];
        emoteObject.GetComponent<SpriteRenderer>().sprite = currentEmote.emoteSprite;
        this.GetComponent<CharMovement>().moveSpeed = currentEmote.moveSpeed;
        rejectDialogueIndex = 0;
    }

    public void UpdateBase()
    {
        if (Time.timeScale != 0)
        {
            //Testing
            if(Input.GetKeyDown(KeyCode.B))
            {
                energyBarSlider.value -= 0.126f;
            }

            if (timeManager.GetCurrentTime() == 0)
            {
                currentTime = timeManager.GetCurrentTime();
            }

            //This value hardcoding is probably dodgy should fix
            if (energyBarSlider.value > 0.622)
            {
                if (currentEmote != charEmotes[(int)EmoteTypes.HAPPY])
                {
                    currentEmote = charEmotes[(int)EmoteTypes.HAPPY];
                    emoteObject.GetComponent<SpriteRenderer>().sprite = currentEmote.emoteSprite;
                    this.GetComponent<CharMovement>().moveSpeed = currentEmote.moveSpeed;
                    DisplayEmoteChangeDialogue(currentEmote.dialogueIndex);
                }
            }

            if (energyBarSlider.value <= 0.622 && energyBarSlider.value > 0.245)
            {
                if (currentEmote != charEmotes[(int)EmoteTypes.TIRED])
                {
                    currentEmote = charEmotes[(int)EmoteTypes.TIRED];
                    emoteObject.GetComponent<SpriteRenderer>().sprite = currentEmote.emoteSprite;
                    this.GetComponent<CharMovement>().moveSpeed = currentEmote.moveSpeed;
                    DisplayEmoteChangeDialogue(currentEmote.dialogueIndex);
                }
            }

            if (energyBarSlider.value <= 0.245)
            {
                if (currentEmote != charEmotes[(int)EmoteTypes.FRUSTRATED])
                {
                    currentEmote = charEmotes[(int)EmoteTypes.FRUSTRATED];
                    emoteObject.GetComponent<SpriteRenderer>().sprite = currentEmote.emoteSprite;
                    this.GetComponent<CharMovement>().moveSpeed = currentEmote.moveSpeed;
                    DisplayEmoteChangeDialogue(currentEmote.dialogueIndex);
                }
            }
        }
    }

    public void UpdateEnergyBar(float multiplier, bool isIncreasing)
    {
        if (isIncreasing)
        {
            energyBarSlider.value = energyBarSlider.value + ((energyCellSize / 4) * multiplier);
        }

        else
        {
            energyBarSlider.value = energyBarSlider.value - ((energyCellSize / 4) * multiplier);
        }
    }

    public void ResetEnergyBar()
    {
        energyBarSlider.value = currentEnergySliderNum;
    }

    public abstract void UpdateBehaviour(float timeVal, float multiplier, bool isEnergyIncreasing);

    public void FullyRestoreEnergy()
    {
        energyBarSlider.value = 1.0f;
    }

    public void DisplayRejectDialogue()
    {
        Player playerScript = this.GetComponent<Player>();

        playerScript.charData.DisplayCharRejectDialogue(rejectDialogueIndex);
        rejectDialogueIndex++;

        if (rejectDialogueIndex == playerScript.charData.rejectDialogueGroups.Count)
        {
            rejectDialogueIndex = 0;
        }
    }

    public void DisplayEmoteChangeDialogue(int index)
    {
        Player playerScript = this.GetComponent<Player>();

        playerScript.charData.DisplayCharEmoteDialogue(index);
    }

    public string GetEmoteAsString()
    {
        return currentEmote.emoteName;
    }
}
