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

    public override void UpdateBehaviour(float timeVal, float multiplier)
    {
        timeManager.AdvanceCurrentTime(timeVal);
        UpdateEnergyBar(multiplier, false);
        currentEnergySliderNum = energyBarSlider.value;
    }
}
