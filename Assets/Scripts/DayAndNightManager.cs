using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DayAndNightManager : MonoBehaviour
{
    [SerializeField] Color nightLightColour;
    [SerializeField] AnimationCurve nightTimeCurve;
    [SerializeField] Color dayLightColour = Color.white;
    [SerializeField] float timeScale = 60f;
    [SerializeField] Light2D globalLight;
    [SerializeField] TextMeshProUGUI timeText;

    private int days = 0;
    const float secondsInDay = 86400f;
    float time;

    float hours
    {
        get { return time / 3600f; }
    }

    float minutes
    {
        get { return time % 3600f / 60f; }
    }

    private void Update()
    {
        time += Time.deltaTime*timeScale;
        int hourNo = (int)hours;
        int minNo = (int)minutes;
        timeText.text = hourNo.ToString("00") + "    :    " + minNo.ToString("00");
        float v = nightTimeCurve.Evaluate(hours);
        Color c = Color.Lerp(dayLightColour, nightLightColour, v);
        globalLight.color = c;

        if(time>secondsInDay)
        {
            NextDay();
        }
    }

    private void NextDay()
    {
        time = 0;
        days += 1;
    }
}
