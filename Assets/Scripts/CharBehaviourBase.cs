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
        if (timeManager.GetCurrentTime() == 0)
        {
            currentTime = timeManager.GetCurrentTime();
        }

        //THIS MIGHT BE DODGY BUT I CANT THINK RN
        if (energyBarSlider.value <= 0.622 && energyBarSlider.value > 0.245)
        {
            emoteObject.GetComponent<SpriteRenderer>().sprite = charEmotes[(int)EmoteTypes.TIRED].emoteSprite;
            this.GetComponent<CharMovement>().moveSpeed = charEmotes[(int)EmoteTypes.TIRED].moveSpeed;
            currentEmote = charEmotes[(int)EmoteTypes.TIRED];
        }

        if (energyBarSlider.value <= 0.245)
        {
            emoteObject.GetComponent<SpriteRenderer>().sprite = charEmotes[(int)EmoteTypes.FRUSTRATED].emoteSprite;
            this.GetComponent<CharMovement>().moveSpeed = charEmotes[(int)EmoteTypes.FRUSTRATED].moveSpeed;
            currentEmote = charEmotes[(int)EmoteTypes.FRUSTRATED];
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
}
