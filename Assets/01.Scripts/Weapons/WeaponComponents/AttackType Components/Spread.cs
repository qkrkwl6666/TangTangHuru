using UnityEngine;

public class Spread : MonoBehaviour
{
    public float spreadAngle = 15f;

    Rigidbody2D rb;
    float timer = 0f;

    IAimer currAimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currAimer = GetComponent<IAimer>();
    }

    private void OnEnable()
    {
        SetFireAngle();
    }

    private void SetFireAngle()
    {
        Vector2 pos = currAimer.AimDirection();
        transform.up = pos;

        float baseAngle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;

        // �߻�ü�� 2�� �̻��� ��� ��� ���� ���
        if (currAimer.TotalCount > 1)
        {
            float totalSpreadAngle = spreadAngle * (currAimer.TotalCount - 1);
            float startAngle = baseAngle - totalSpreadAngle / 2f;
            float angleStep = totalSpreadAngle / (currAimer.TotalCount - 1);

            float currentAngle = startAngle + angleStep * currAimer.Index;
            float radianAngle = currentAngle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle));

            rb.velocity = direction * currAimer.Speed;
        }
        else
        {
            rb.velocity = pos * currAimer.Speed;
        }
    }

    private void OnDisable()
    {
        timer = 0f;
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
    }

}
