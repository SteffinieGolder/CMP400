using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//Script which controls the tasks in the game. 

public class TaskManager : MonoBehaviour
{
    //Character tasks, totals and icons.
    public List<TextMeshProUGUI> char1Tasks;
    public List<TextMeshProUGUI> char1Totals;
    public List<Image> char1Icons;
    public List<TextMeshProUGUI> char2Tasks;
    public List<TextMeshProUGUI> char2Totals;
    public List<Image> char2Icons;
    public Sprite completeSprite;

    //Counters and bools for tasks.
    public int fishTaskCounter = 0;
    public int weedTaskCounter = 0;
    public int hoeTaskCounter = 0;
    public int wateredSeedCounter = 0;
    public bool isFishingComplete = false;
    public bool isPlantingComplete = false;
    public bool isSoilPrepComplete = false;
    public bool hasFishingStarted = false;
    public bool hasPlantingStarted = false;
    public bool hasWeedingStarted = false;
    public bool isWeedingComplete = false;
    public bool hasPlayerDrunkCoffee = false;
    //Total task counter.
    public int totalTaskCounter = 4;

    //Reset all task variables.
    public void ResetCounters()
    {
        fishTaskCounter = 0;
        weedTaskCounter = 0;
        hoeTaskCounter = 0;
        wateredSeedCounter = 0;
        totalTaskCounter = 4;
        isFishingComplete = false;
        isPlantingComplete = false;
        hasFishingStarted = false;
        hasPlantingStarted = false;
        hasWeedingStarted = false;
        isWeedingComplete = false;
        hasPlayerDrunkCoffee = false;
        isSoilPrepComplete = false;
}

    //Returns if a task stage has been completed or not.
    public bool IsTaskPortionComplete(bool isChar1, int taskIndex)
    {
        //Checks character one's tasks.
        if (isChar1)
        {
            int currentNum = int.Parse(char1Tasks[taskIndex].text);
            int totalNum = int.Parse(char1Totals[taskIndex].text);

            currentNum++;

            //Increments the task counter text on the UI.
            if (currentNum <= totalNum)
            {
                char1Tasks[taskIndex].text = currentNum.ToString();
                return true;
            }

            return false;
        }

        //Checks character 2's tasks.
        else
        {
            int currentNum = int.Parse(char2Tasks[taskIndex].text);
            int totalNum = int.Parse(char2Totals[taskIndex].text);

            currentNum++;

            //Increments the task counter text on the UI.
            if (currentNum <= totalNum)
            {
                char2Tasks[taskIndex].text = currentNum.ToString();
                return true;
            }

            return false;
        }
    }

    //Returns if all task stages have been complete or not.
    public bool IsTaskTotallyComplete(bool isChar1, int taskIndex)
    {
        //Checks character one's tasks.
        if (isChar1)
        {
            int currentNum = int.Parse(char1Tasks[taskIndex].text);
            int totalNum = int.Parse(char1Totals[taskIndex].text);

            //Updates task number text and changes the task sprite to a checkmark.
            if (currentNum == totalNum)
            {
                char1Tasks[taskIndex].text = currentNum.ToString();
                char1Icons[taskIndex].sprite = completeSprite;
                return true;
            }
        }

        //Checks character two's tasks.
        else
        {
            int currentNum = int.Parse(char2Tasks[taskIndex].text);
            int totalNum = int.Parse(char2Totals[taskIndex].text);

            //Updates task number text and changes the task sprite to a checkmark.
            if (currentNum == totalNum)
            {
                char2Tasks[taskIndex].text = currentNum.ToString();
                char2Icons[taskIndex].sprite = completeSprite;
                return true;
            }
        }

        return false;
    }

    //Returns the amount of completed tasks for the active character.
    public int GetAmountOfCompletedTasks(bool isChar1, int taskIndex)
    {
        if (isChar1)
        {
            int currentNum = int.Parse(char1Tasks[taskIndex].text);
            return currentNum;
        }

        else
        {
            int currentNum = int.Parse(char2Tasks[taskIndex].text);
            return currentNum;
        }
    }
}

