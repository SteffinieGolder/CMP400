using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

//A LOT OF THESE FUNCTIONS COULD BE CONDENSED REMOVE REPEATED CODE

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
    private int shouldBeFishingIndex;
    private int busyFishingIndex;
    private int shouldBePlantingIndex;
    private int busyPlantingIndex;
    private int findAxeIndex;

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
        shouldBeFishingIndex = 0;
        busyFishingIndex = 0;
        shouldBePlantingIndex = 0;
        busyPlantingIndex = 0;

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
            if (energyBarSlider.value > charEmotes[(int)EmoteTypes.HAPPY].lowerLimit)
            {
                if (currentEmote != charEmotes[(int)EmoteTypes.HAPPY])
                {
                    currentEmote = charEmotes[(int)EmoteTypes.HAPPY];
                    emoteObject.GetComponent<SpriteRenderer>().sprite = currentEmote.emoteSprite;
                    this.GetComponent<CharMovement>().moveSpeed = currentEmote.moveSpeed;
                    DisplayEmoteChangeDialogue(currentEmote.dialogueIndex);
                }
            }

            else if (energyBarSlider.value <= charEmotes[(int)EmoteTypes.TIRED].upperLimit && energyBarSlider.value > charEmotes[(int)EmoteTypes.TIRED].lowerLimit)
            {
                if (currentEmote != charEmotes[(int)EmoteTypes.TIRED])
                {
                    currentEmote = charEmotes[(int)EmoteTypes.TIRED];
                    emoteObject.GetComponent<SpriteRenderer>().sprite = currentEmote.emoteSprite;
                    this.GetComponent<CharMovement>().moveSpeed = currentEmote.moveSpeed;
                    DisplayEmoteChangeDialogue(currentEmote.dialogueIndex);
                }
            }

           else if (energyBarSlider.value <= charEmotes[(int)EmoteTypes.HAPPY].lowerLimit)
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

    public void DisplayFindAxeDialogue()
    {
        Player playerScript = this.GetComponent<Player>();

        playerScript.charData.DisplayFindAxeDialogue(findAxeIndex);
        findAxeIndex++;

        if (findAxeIndex == playerScript.charData.findAxeDialogue.Count)
        {
            findAxeIndex = 0;
        }
    }

    public void DisplayEmoteChangeDialogue(int index)
    {
        Player playerScript = this.GetComponent<Player>();

        playerScript.charData.DisplayCharEmoteDialogue(index);
    }

    public void DisplayBusyOrFinishedFishingDialogue()
    {
        Player playerScript = this.GetComponent<Player>();

        playerScript.charData.DisplayFishingDialogue(busyFishingIndex);
        busyFishingIndex++;

        if (busyFishingIndex == playerScript.charData.busyOrFinishedFishingDialogue.Count)
        {
            busyFishingIndex = 0;
        }
    }

    public void DisplayShouldBeFishingDialogue()
    {
        Player playerScript = this.GetComponent<Player>();

        playerScript.charData.DisplayShouldBeFishingDialogue(shouldBeFishingIndex);
        shouldBeFishingIndex++;

        if (shouldBeFishingIndex == playerScript.charData.shouldBeFishingDialogue.Count)
        {
            shouldBeFishingIndex = 0;
        }
    }

    public void DisplayBusyOrFinishedPlantingDialogue()
    {
        Player playerScript = this.GetComponent<Player>();

        playerScript.charData.DisplayPlantingDialogue(busyPlantingIndex);
        busyPlantingIndex++;

        if (busyPlantingIndex == playerScript.charData.busyOrFinishedPlantingDialogue.Count)
        {
            busyPlantingIndex = 0;
        }
    }

    public void DisplayShouldBePlantingDialogue()
    {
        Player playerScript = this.GetComponent<Player>();

        playerScript.charData.DisplayShouldBePlantingDialogue(shouldBePlantingIndex);
        shouldBePlantingIndex++;

        if (shouldBePlantingIndex == playerScript.charData.shouldBePlantingDialogue.Count)
        {
            shouldBePlantingIndex = 0;
        }
    }

    public string GetEmoteAsString()
    {
        return currentEmote.emoteName;
    }
}
