using UnityEngine;

public class WavingEffect : MonoBehaviour
{
    public float speed = 2f; // �¿� ������ �ӵ�
    public float distance = 1f; // �¿� ������ �Ÿ�

    private Vector3 initialPosition;

    void Start()
    {
        // �ڽ� ������Ʈ�� �ʱ� ��ġ�� ����
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        // �ڽ� ������Ʈ�� �¿�� �����̱�
        float offset = Mathf.PingPong(Time.time * speed, distance) - (distance / 2f);
        transform.localPosition = initialPosition + new Vector3(offset, 0, 0);
    }
}
