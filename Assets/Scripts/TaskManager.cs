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

    public bool IsTaskComplete(bool isChar1, int taskIndex)
    {
        if (isChar1)
        {
            int currentNum = int.Parse(char1Tasks[taskIndex].text);
            int totalNum = int.Parse(char1Totals[taskIndex].text);

            currentNum++;

            if(currentNum < totalNum)
            {
                char1Tasks[taskIndex].text = currentNum.ToString();
                return true;
            }

            if (currentNum == totalNum)
            {
                char1Tasks[taskIndex].text = currentNum.ToString();
                char1Icons[taskIndex].sprite = completeSprite;
                return true;
            }

            return false;
        }

        else
        {
            int currentNum = int.Parse(char2Tasks[taskIndex].text);
            int totalNum = int.Parse(char2Totals[taskIndex].text);

            currentNum++;

            if (currentNum < totalNum)
            {
                char2Tasks[taskIndex].text = currentNum.ToString();
                return true;
            }

            if (currentNum == totalNum)
            {
                char2Tasks[taskIndex].text = currentNum.ToString();
                char2Icons[taskIndex].sprite = completeSprite;
                return true;
            }

            return false;
        }
    }
}