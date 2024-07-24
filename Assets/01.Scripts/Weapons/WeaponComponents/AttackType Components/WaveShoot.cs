using UnityEngine;

public class WaveShoot : MonoBehaviour, IProjectile
{
    public IAimer currAimer;

    private float timer = 0f;
    private Rigidbody2D rb;

    //
    private Vector2 initialPosition;
    private Vector2 targetPosition;

    private float frequency = 15f; // �ⷷ�̴� ��
    private float magnitude = 2f; // �ⷷ�̴� ����
    private float currFrequency;

    public float Range { get; set; }
    public float Size { get; set; }
    public float Speed { get; set; }

    //

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currAimer = GetComponent<IAimer>();
    }

    private void OnEnable()
    {
        timer = 0f;
        transform.localScale = new Vector3(Size, Size);

        var pos = currAimer.AimDirection();
        transform.up = pos;
        rb.velocity = pos * Speed;

        initialPosition = transform.position;
        targetPosition = initialPosition + (Vector2)pos * Speed * currAimer.LifeTime;

        var dir = (currAimer.Index % 2) == 0 ? -1f : 1f;
        currFrequency = frequency * dir;
    }

    private void OnDisable()
    {
    }

    private void Update()
    {
        if (timer > currAimer.LifeTime)
        {
            gameObject.SetActive(false);
        }
        else
        {
            timer += Time.deltaTime;
        }

        float progress = timer / currAimer.LifeTime;
        Vector2 linearPosition = Vector2.Lerp(initialPosition, targetPosition, progress);

        // �����ķ� �ⷷ�̴� ȿ�� �߰�
        float sineOffset = Mathf.Sin(timer * currFrequency) * magnitude;
        Vector2 perpendicular = Vector2.Perpendicular((targetPosition - initialPosition).normalized);
        Vector2 sineWavePosition = linearPosition + perpendicular * sineOffset;

        transform.position = sineWavePosition;
    }
}
