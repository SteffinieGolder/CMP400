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

        if (Input.GetKeyDown(KeyCode.J))
        {
            UpdateEnergyBar(3, false);
            currentEnergySliderNum = energyBarSlider.value;
        }
    }
}
