using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskController : MonoBehaviour
{
    public List<TextMeshProUGUI> char1Tasks;
    public List<TextMeshProUGUI> char2Tasks;
    public Sprite completeSprite;

    public void UpdateTask(bool isChar1, int taskIndex, int numCompleted)
    {
        //GET TOTAL NUM TEXT FROM THE TASK AND CHECK IF ITS COMPLETE USING PARAM
        if(isChar1)
        {
           char1Tasks[taskIndex].text = numCompleted.ToString();
        }

        else
        {
            char2Tasks[taskIndex].text = numCompleted.ToString();
        }
    }

    public void CompleteTask(bool isChar1, int index)
    {
        if(isChar1)
        {
            //char1Tasks[index].GetComponentInParent<Image>().sprite = completeSprite;
        }

        else
        {
            //char2Tasks[index].GetComponentInParent<Image>().sprite = completeSprite;
        }
    }

}
