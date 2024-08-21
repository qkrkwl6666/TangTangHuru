using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExp : MonoBehaviour
{
    private float currExp;
    public float CurrExp { get => currExp; }

    public float requiredExp = 1000f;

    public int levelinStage = 0;

    public event Action<int> OnLevelChanged;

    private List<float> requiredExps;

    private void Awake()
    {
        //경험치 테이블 받아오기
    }


    public void EarnExp(float exp)
    {
        currExp += exp;

        //Debug.Log("현재 경험치 : " + currExp);

        if (currExp > requiredExp)
        {
            currExp -= requiredExp;

            requiredExp += (requiredExp * 0.1f);

            //레벨업 메소드 호출
            levelinStage++;
            OnLevelChanged?.Invoke(levelinStage);
        }
    }

    public float GetRequiredExp()
    {
        return requiredExp;
    }


}
