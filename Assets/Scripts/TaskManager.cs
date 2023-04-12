using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public List<TextMeshProUGUI> char1Tasks;
    public List<TextMeshProUGUI> char1Totals;
    public List<Image> char1Icons;
    public List<TextMeshProUGUI> char2Tasks;
    public List<TextMeshProUGUI> char2Totals;
    public List<Image> char2Icons;
    public Sprite completeSprite;

    //public List<float> NTCharTimes;
    //public List<float> ADHDCharTimes;

    public int fishTaskCounter = 0;
    public int weedTaskCounter = 0;
    public int hoeTaskCounter = 0;
    public bool isFishingComplete = false;
    public bool isPlantingComplete = false;
    public bool hasFishingStarted = false;
    public bool hasPlantingStarted = false;
    public bool hasWeedingStarted = false;
    public bool isWeedingComplete = false;

    //int totalCompleteTasks = 0;

    private void Start()
    {
        fishTaskCounter = 0;
        weedTaskCounter = 0;
        hoeTaskCounter = 0;
        //totalCompleteTasks = 0;
    }

    public bool IsTaskPortionComplete(bool isChar1, int taskIndex)
    {
        if (isChar1)
        {
            int currentNum = int.Parse(char1Tasks[taskIndex].text);
            int totalNum = int.Parse(char1Totals[taskIndex].text);

            currentNum++;

            if(currentNum <= totalNum)
            {
                char1Tasks[taskIndex].text = currentNum.ToString();
                return true;
            }

            return false;
        }

        else
        {
            int currentNum = int.Parse(char2Tasks[taskIndex].text);
            int totalNum = int.Parse(char2Totals[taskIndex].text);

            currentNum++;

            if (currentNum <= totalNum)
            {
                char2Tasks[taskIndex].text = currentNum.ToString();
                return true;
            }

            return false;
        }
    }

    public bool IsTaskTotallyComplete(bool isChar1, int taskIndex)
    {
        if (isChar1)
        {
            int currentNum = int.Parse(char1Tasks[taskIndex].text);
            int totalNum = int.Parse(char1Totals[taskIndex].text);

            if (currentNum == totalNum)
            {
                char1Tasks[taskIndex].text = currentNum.ToString();
                char1Icons[taskIndex].sprite = completeSprite;
                return true;
            }
        }

        else
        {
            int currentNum = int.Parse(char2Tasks[taskIndex].text);
            int totalNum = int.Parse(char2Totals[taskIndex].text);

            if (currentNum == totalNum)
            {
                char2Tasks[taskIndex].text = currentNum.ToString();
                char2Icons[taskIndex].sprite = completeSprite;
                return true;
            }
        }

        return false;
    }

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

    /*public void AdvanceTimeForward()
    {
        if(GameManager.instance.characterManager.char1IsActive)
        {
            GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().AdvanceTime(ADHDCharTimes[totalCompleteTasks]);
        }

        else
        {
            GameManager.instance.characterManager.activePlayer.GetComponent<CharBehaviourBase>().AdvanceTime(NTCharTimes[totalCompleteTasks]);
        }

        totalCompleteTasks++;
    }*/
}

