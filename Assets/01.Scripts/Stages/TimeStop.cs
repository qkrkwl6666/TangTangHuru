using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : MonoBehaviour
{
    public TimeControl TimeController;
    private void OnEnable()
    {
        TimeController.StopTime();
    }
    private void OnDisable()
    {
        TimeController.ResumeTime();
    }
}
