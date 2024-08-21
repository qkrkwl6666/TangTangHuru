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
        //����ġ ���̺� �޾ƿ���
    }


    public void EarnExp(float exp)
    {
        currExp += exp;

        //Debug.Log("���� ����ġ : " + currExp);

        if (currExp > requiredExp)
        {
            currExp -= requiredExp;

            requiredExp += (requiredExp * 0.1f);

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
