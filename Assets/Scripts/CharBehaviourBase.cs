using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

//Script which controls the character behaviour (energy levels, emotes, dialogue etc).
public class CharBehaviourBase : MonoBehaviour
{
    //Energy bar.
    public Slider energyBarSlider;
    //List of emotes for this character.
    public List<EmoteData> charEmotes;
    //The emote object.
    //public GameObject emoteObject;
    //A cell on the energy bar.
    protected float energyCellSize = 0.126f;
    //The current energy slider value (1 is full).
    protected float currentEnergySliderNum = 1;
    //Current day time.
    protected float currentTime;
    //Time manager class.
    protected DayAndNightManager timeManager;
    //The current emote for this character.
    public EmoteData currentEmote;

    //Indexes for different dialogue groups.
    private int rejectDialogueIndex;
    private int shouldBeFishingIndex;
    private int busyFishingIndex;
    private int shouldBePlantingIndex;
    private int busyPlantingIndex;
    private int findAxeIndex;

    //Types of emotes and indexes in emote list.
    enum EmoteTypes
    {
        HAPPY = 0,
        TIRED = 1,
        FRUSTRATED = 2
    }

    //Initialise variables.
    private void Start()
    {
        currentEnergySliderNum = energyBarSlider.value;
        timeManager = GameManager.instance.dayAndNightManager;
        currentTime = timeManager.GetCurrentTime();

        currentEmote = charEmotes[(int)EmoteTypes.HAPPY];
        //emoteObject.GetComponent<SpriteRenderer>().sprite = currentEmote.emoteSprite;
        this.GetComponent<CharMovement>().moveSpeed = currentEmote.moveSpeed;

        rejectDialogueIndex = 0;
        shouldBeFishingIndex = 0;
        busyFishingIndex = 0;
        shouldBePlantingIndex = 0;
        busyPlantingIndex = 0;
    }

    private void Update()
    {
        //Run if game isn't paused.
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

            //Changes the character emote to happy if their energy levels are within the happy range.
            if (energyBarSlider.value > charEmotes[(int)EmoteTypes.HAPPY].lowerLimit)
            {
                if (currentEmote != charEmotes[(int)EmoteTypes.HAPPY])
                {
                    //Update emote sprite and move speed.
                    currentEmote = charEmotes[(int)EmoteTypes.HAPPY];
                   // emoteObject.GetComponent<SpriteRenderer>().sprite = currentEmote.emoteSprite;
                    this.GetComponent<CharMovement>().moveSpeed = currentEmote.moveSpeed;
                    //Display emote change dialogue.
                    DisplayEmoteChangeDialogue(currentEmote.dialogueIndexPreCoffee);
                }
            }

            //Changes the character emote to tired if their energy levels are within the tired range.
            else if (energyBarSlider.value <= charEmotes[(int)EmoteTypes.TIRED].upperLimit && energyBarSlider.value > charEmotes[(int)EmoteTypes.TIRED].lowerLimit)
            {
                if (currentEmote != charEmotes[(int)EmoteTypes.TIRED])
                {
                    currentEmote = charEmotes[(int)EmoteTypes.TIRED];
                   // emoteObject.GetComponent<SpriteRenderer>().sprite = currentEmote.emoteSprite;
                    this.GetComponent<CharMovement>().moveSpeed = currentEmote.moveSpeed;

                    //Display emote change dialogue depending on if player has drunk coffee yet in game.
                    if (GameManager.instance.taskManager.hasPlayerDrunkCoffee)
                    {
                        DisplayEmoteChangeDialogue(currentEmote.dialogueIndexPostCoffee);
                    }

                    else
                    {
                        DisplayEmoteChangeDialogue(currentEmote.dialogueIndexPreCoffee);
                    }
                }
            }

            //Changes the character emote to frustrated if their energy levels are within the frustrated range.
            else if (energyBarSlider.value <= charEmotes[(int)EmoteTypes.HAPPY].lowerLimit)
            {
                if (currentEmote != charEmotes[(int)EmoteTypes.FRUSTRATED])
                {
                    currentEmote = charEmotes[(int)EmoteTypes.FRUSTRATED];
                    //emoteObject.GetComponent<SpriteRenderer>().sprite = currentEmote.emoteSprite;
                    this.GetComponent<CharMovement>().moveSpeed = currentEmote.moveSpeed;

                    //Display emote change dialogue depending on if player has drunk coffee yet in game.
                    if (GameManager.instance.taskManager.hasPlayerDrunkCoffee)
                    {
                        DisplayEmoteChangeDialogue(currentEmote.dialogueIndexPostCoffee);
                    }

                    else
                    {
                        DisplayEmoteChangeDialogue(currentEmote.dialogueIndexPreCoffee);
                    }
                }
            }
        }
    }

    //Change the value of the energy bar using params. Energy can be increased or decreased.
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

    //Reset the energy bar.
    public void ResetEnergyBar()
    {
        energyBarSlider.value = currentEnergySliderNum;
    }

    //Update the character behaviour. Advance time and update their energy bar.
    public void UpdateBehaviour(float timeVal, float multiplier, bool isEnergyIncreasing)
    {
        timeManager.AdvanceCurrentTime(timeVal);
        UpdateEnergyBar(multiplier, isEnergyIncreasing);
        currentEnergySliderNum = energyBarSlider.value;
    }

    //Restore energy to full.
    public void FullyRestoreEnergy()
    {
        energyBarSlider.value = 1.0f;
    }

    //Set energy level to param.
    public void SetEnergyLevel(float value)
    {
        energyBarSlider.value = value;
    }

    //Display the dialogue which rejects a task.
    public void DisplayRejectDialogue()
    {
        Player playerScript = this.GetComponent<Player>();

        //Access the dialogue group through the characters data object.
        playerScript.charData.DisplayCharRejectDialogue(rejectDialogueIndex);
        //Increase the index so the next line can be accessed.
        rejectDialogueIndex++;

        //If the index has reached the end of the collection of dialogue lines to show then reset it.
        if (rejectDialogueIndex == playerScript.charData.rejectDialogueGroups.Count)
        {
            rejectDialogueIndex = 0;
        }
    }

    //Display the dialogue which tells the user to find the axe.
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

    //Display the emote change dialogue.
    public void DisplayEmoteChangeDialogue(int index)
    {
        Player playerScript = this.GetComponent<Player>();

        playerScript.charData.DisplayCharEmoteDialogue(index);
    }

    //Display the dialogue for the fishing task.
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

    //Display the dialogue which tells the user they should be fishing.
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

    //Displays the dialogue which tells the user to go back to planting.
    public void DisplayBusyPlantingDialogue()
    {
        Player playerScript = this.GetComponent<Player>();

        playerScript.charData.DisplayPlantingDialogue(busyPlantingIndex);
        busyPlantingIndex++;

        if (busyPlantingIndex == playerScript.charData.busyPlantingDialogue.Count)
        {
            busyPlantingIndex = 0;
        }
    }

    //Display the dialogue which tells the user they should start planting seeds.
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

    //Return the current emote.
    public string GetEmoteAsString()
    {
        return currentEmote.emoteName;
    }

    //Advance time forward by param.
    public void AdvanceTime(float timeToAdvanceBy)
    {
        timeManager.AdvanceCurrentTime(timeToAdvanceBy);
    }
}
