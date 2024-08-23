using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExp : MonoBehaviour
{
    private float currExp;
    public float CurrExp { get => currExp; }

    public float requiredExp = 1500f;

    public int levelinStage = 0;

    public event Action<int> OnLevelChanged;

    private List<float> requiredExps;

    private void Awake()
    {
        //����ġ ���̺� �޾ƿ���
    }


    public void EarnExp(float exp)
    {
        currExp += exp;

        if (currExp > requiredExp)
        {
            currExp -= requiredExp;

            requiredExp += (requiredExp * 0.2f);

            //������ �޼ҵ� ȣ��
            levelinStage++;
            OnLevelChanged?.Invoke(levelinStage);
        }
    }

    public float GetRequiredExp()
    {
        return requiredExp;
    }


}
