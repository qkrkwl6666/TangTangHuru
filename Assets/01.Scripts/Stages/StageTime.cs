using TMPro;
using UnityEngine;

public class StageTime : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public MonsterSpawnManager monsterSpawnManager;

    public bool IsStop { get; private set; }

    private readonly float endTime = 10f;

    float currTime = 0f;
    string minuteText;
    string secondText;

    void Start()
    {
        currTime = 0f;
    }

    private void Update()
    {
        if (IsStop) return;

        currTime += Time.deltaTime;

        if (currTime >= endTime)
        {
            monsterSpawnManager.OnStop?.Invoke();
            IsStop = true;
            currTime = endTime;
        }

        var minute = currTime / 60;
        var second = currTime % 60;

        minuteText = Mathf.FloorToInt(minute).ToString("D2");
        secondText = Mathf.FloorToInt(second).ToString("D2");

        timeText.text = minuteText + ":" + secondText;
    }
}
