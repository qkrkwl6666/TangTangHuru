using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExp : MonoBehaviour
{
    private float currExp;
    public float CurrExp { get => currExp; }

    public float requiredExp = 1000f;

    private List<float> requiredExps;

    private void Awake()
    {
        //����ġ ���̺� �޾ƿ���
    }


    public void EarnExp(float exp)
    {
        currExp += exp;

        Debug.Log("���� ����ġ : " + currExp);

        if(currExp > requiredExp)
        {
            currExp -= requiredExp;

            //������ �޼ҵ� ȣ��
        }
    }


}
