using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureBar : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void SetActiveTreasureBar(bool active)
    {
        gameObject.SetActive(active);
    }
    public void UpdateTreasureBar(float value)
    {
        slider.value = value;
    }
}
