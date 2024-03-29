using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

//Script which controls the day and night cycle in the game. This changes the lighting using an animation curve depending on the in game time. 
//Code adapted from this tutorial series by Greg Dev Stuff: https://www.youtube.com/watch?v=ZIEE-2ZdAxU&list=PL0GUZtUkX6t6wXF0U0WAQNVYL68pYUCZv&ab_channel=GregDevStuff 

public class DayAndNightManager : MonoBehaviour
{
    //Light colours and Animation curve which controls transition between them. 
    [SerializeField] Color nightLightColour;
    [SerializeField] AnimationCurve nightTimeCurve;
    [SerializeField] Color dayLightColour = Color.white;
    //Time scale to control how quick a day passes in game. 
    public float timeScale = 20f;
    //Light
    [SerializeField] Light2D globalLight;
    //Text for time display on screen. 
    [SerializeField] TextMeshProUGUI timeText;

    //Total days counter.
    private int days = 0;
    //Amount of seconds in a day. 
    const float secondsInDay = 86400f;
    //Game time variable.
    public float startTime = 21600f;
    float time;

    bool once = false;

    //Time in hours.
    float hours
    {
        get { return time / 3600f; }
    }

    //Time in minutes. 
    float minutes
    {
        get { return time % 3600f / 60f; }
    }

    private void Start()
    {
        //Set time at start of game to be 6am.
        time = startTime;    
    }

    private void Update()
    {
        //Set the time to delta multiplied by the timescale (higher number speeds up time passing).
        time += Time.deltaTime*timeScale;
        int hourNo = (int)hours;
        int minNo = (int)minutes;
        //Time text object displays hours and minutes. 
        timeText.text = hourNo.ToString("00") + "    :    " + minNo.ToString("00");

        //Get animation curve value at current hour and transition light colour depending on result. 
        float v = nightTimeCurve.Evaluate(hours);
        Color c = Color.Lerp(dayLightColour, nightLightColour, v);
        globalLight.color = c;

        //If end of day has been reached, transition to next day.
        if(time>secondsInDay)
        {
            NextDay();
        }
    }

    private void NextDay()
    {
        //Reset time and increment days variable by 1.
        time = 0;
        days += 1;
    }

    //Returns the current time.
    public float GetCurrentTime()
    {
        if(!once)
        {
            once = true;
            return startTime;
        }

        return time;
    }
    
    //Advances time forward by the amount passed in.
    public void AdvanceCurrentTime(float timeVal)
    {
        time += timeVal;
    }

    //Sets the current time to the amount passed in.
    public void SetTime(float timeToSkipTo)
    {
        time = timeToSkipTo;
    }
}
