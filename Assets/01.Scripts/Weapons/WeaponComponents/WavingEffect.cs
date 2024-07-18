using UnityEngine;

public class WavingEffect : MonoBehaviour
{
    public float speed = 2f; // 좌우 움직임 속도
    public float distance = 1f; // 좌우 움직임 거리

    private Vector3 initialPosition;

    void Start()
    {
        // 자식 오브젝트의 초기 위치를 저장
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        // 자식 오브젝트를 좌우로 움직이기
        float offset = Mathf.PingPong(Time.time * speed, distance) - (distance / 2f);
        transform.localPosition = initialPosition + new Vector3(offset, 0, 0);
    }
}
