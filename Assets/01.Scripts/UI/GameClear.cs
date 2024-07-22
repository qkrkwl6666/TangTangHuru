using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameClear : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI killText;

    public void SetGameClear(int gold, int kill)
    {
        goldText.text = gold.ToString();
        killText.text = kill.ToString();
    }
}
