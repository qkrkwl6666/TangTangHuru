using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageTime : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    float currTime = 0f;
    string minuteText;
    string secondText; 

    void Start()
    {
        currTime = 0f;
    }

    private void Update()
    {
        currTime += Time.deltaTime;
        var minute = currTime / 60;
        var second = currTime % 60;

        minuteText = Mathf.FloorToInt(minute).ToString("D2");
        secondText = Mathf.FloorToInt(second).ToString("D2");

        timeText.text = minuteText+":" + secondText;
    }
}
