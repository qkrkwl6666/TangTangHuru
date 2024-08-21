using UnityEngine;

public class TimeControl : MonoBehaviour
{
    float currTime = 1.0f;
    float prevTime;
    float doubleTime = 2.0f;
    float normalTime = 1.0f;
    float stopTime = 0f;

    void Start()
    {
        //currTime = 1.0f;
        //Time.timeScale = 1.0f;
    }

    public void StopTime()
    {
        prevTime = currTime;
        Time.timeScale = stopTime;
        currTime = Time.timeScale;
    }

    public void ResumeTime()
    {
        Time.timeScale = prevTime;
        currTime = Time.timeScale;
    }

    public void DoubleTime()
    {
        prevTime = currTime;
        Time.timeScale = doubleTime;
        currTime = Time.timeScale;
    }

    public void NormalTime()
    {
        prevTime = currTime;
        Time.timeScale = normalTime;
        currTime = Time.timeScale;
    }
}
