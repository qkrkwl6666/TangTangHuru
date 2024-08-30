using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevelopMode : MonoBehaviour
{
    // Start is called before the first frame update

    public Button timeScaleButton;

    private void Awake()
    {
        timeScaleButton.onClick.AddListener(TimeScaleMode);
    }

    public void TimeScaleMode()
    {
        Time.timeScale = 10f;
    }
}
