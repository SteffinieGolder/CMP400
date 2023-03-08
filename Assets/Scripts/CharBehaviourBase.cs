using UnityEngine;
using UnityEngine.UI;

public abstract class CharBehaviourBase : MonoBehaviour
{
    public Slider energyBarSlider;
    protected float energyCellSize = 0.126f;
    protected float currentEnergySliderNum = 1;
    protected float timeBetweenChanges = 8000;
    protected float currentTime;
    protected DayAndNightManager timeManager;

    public void Initialise()
    {
        currentEnergySliderNum = energyBarSlider.value;
        timeManager = GameManager.instance.dayAndNightManager;
        currentTime = timeManager.GetCurrentTime();
    }

    public void UpdateBase()
    {
        if (timeManager.GetCurrentTime() == 0)
        {
            currentTime = timeManager.GetCurrentTime();
        }
    }

    public void UpdateEnergyBar(int multiplier, bool isIncreasing)
    {
        if(isIncreasing)
        {
            energyBarSlider.value = energyBarSlider.value + (energyCellSize * multiplier);
        }

        else
        {
            energyBarSlider.value = energyBarSlider.value - (energyCellSize * multiplier);
        }
    }

    public void ResetEnergyBar()
    {
        energyBarSlider.value = currentEnergySliderNum;
    }

    public abstract void UpdateTime(float timeVal);
}
