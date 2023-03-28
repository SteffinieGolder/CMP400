using UnityEngine;

public class ADHDCharBehaviour : CharBehaviourBase
{
    void Start()
    {
        Initialise();
    }

    void Update()
    {
        base.UpdateBase();
    }

    public override void UpdateBehaviour(float timeVal, float multiplier, bool isEnergyIncreasing)
    {
        timeManager.AdvanceCurrentTime(timeVal);
        UpdateEnergyBar(multiplier, isEnergyIncreasing);
        currentEnergySliderNum = energyBarSlider.value;
    }
}
