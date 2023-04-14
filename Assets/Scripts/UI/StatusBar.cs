using UnityEngine;
using UnityEngine.UI;

//Script which controls the energy bar UI. 

public class StatusBar : MonoBehaviour
{
    //The slider object on the UI.
    [SerializeField] Slider barSlider;

    //Sets the slider value based on params. 
    public void SetBarValue(int current, int max)
    {
        barSlider.maxValue = max;
        barSlider.value = current;
    }

}
