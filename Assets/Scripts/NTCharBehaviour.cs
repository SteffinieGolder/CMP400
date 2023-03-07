public class NTCharBehaviour : CharBehaviourBase
{
    void Start()
    {
        Initialise();
    }

    void Update()
    {
        base.UpdateBase();

        if (timeManager.GetCurrentTime() >= (currentTime+timeBetweenChanges))
        {
            UpdateEnergyBar(1, false);
            currentEnergySliderNum = energyBarSlider.value;
            currentTime = timeManager.GetCurrentTime();
        }
    }
}
