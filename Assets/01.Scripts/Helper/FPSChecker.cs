using UnityEngine;

public class FPSChecker : MonoBehaviour
{
    private int fFont_Size = 50;
    [Range(0, 1)]
    public float Red, Green, Blue;

    float deltaTime = 0.0f;

    float time = 0f;
    float timetotal = 0f;
    float fpsDuration = 60f;
    float totalFps = 0f;
    bool isFps = false;

    private void Start()
    {
        fFont_Size = fFont_Size == 0 ? 50 : fFont_Size;
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        time += Time.deltaTime;
        timetotal += Time.deltaTime;

        if (time >= fpsDuration && !isFps)
        {
            Debug.Log($"1분 평균 프레임 : {totalFps / 60}");
            isFps = true;
        }
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 0.02f);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / fFont_Size;
        style.normal.textColor = new Color(Red, Green, Blue, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);

        if (timetotal >= 1f)
        {
            totalFps += fps;
            timetotal = 0f;
        }
    }
}
