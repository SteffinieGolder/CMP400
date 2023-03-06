using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField] Slider barSlider;

    public void SetBarValue(int current, int max)
    {
        barSlider.maxValue = max;
        barSlider.value = current;
    }

}
