using UnityEngine;

public class LocalFix : MonoBehaviour
{
    public Vector3 fixedRotation;

    void LateUpdate()
    {
        transform.localRotation = Quaternion.Euler(fixedRotation); // 원하는 회전값으로 설정
    }
}
