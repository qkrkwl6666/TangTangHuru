using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalFix : MonoBehaviour
{
    public Vector3 fixedRotation;

    void LateUpdate()
    {
        transform.localRotation = Quaternion.Euler(fixedRotation); // ���ϴ� ȸ�������� ����
    }
}
