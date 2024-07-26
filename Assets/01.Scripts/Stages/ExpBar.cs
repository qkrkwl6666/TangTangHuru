using System;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    public Slider expSlider;

    PlayerExp playerExp;

    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerExp = player.GetComponent<PlayerExp>();
            UpdateMaxValue(1);
        }
        else
        {
            Debug.LogError("No Player Here!");
        }

        playerExp.OnLevelChanged += UpdateMaxValue;
    }


    // Todo : 나중에 옵저버 패턴으로 갱신 될때만 가져오기
    private void FixedUpdate()
    {
        expSlider.value = playerExp.CurrExp;
    }

    public void UpdateMaxValue(int level)
    {
        expSlider.maxValue = playerExp.requiredExp;
        expSlider.value = playerExp.CurrExp;
    }


}
