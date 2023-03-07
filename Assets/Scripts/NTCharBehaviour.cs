public class NTCharBehaviour : CharBehaviourBase
{
    void Start()
    {
        Initialise();
    }

    void Update()
    {
        if(timeManager.GetCurrentTime() == 0)
        {
            currentTime = timeManager.GetCurrentTime();
        }

        if (timeManager.GetCurrentTime() >= (currentTime+timeBetweenChanges))
        {
            UpdateEnergyBar(1, false);
            currentEnergySliderNum = energyBarSlider.value;
            currentTime = timeManager.GetCurrentTime();
        }
    }
}
