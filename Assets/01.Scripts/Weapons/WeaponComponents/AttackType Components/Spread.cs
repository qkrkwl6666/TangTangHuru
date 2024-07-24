using UnityEngine;

public class Spread : MonoBehaviour, IProjectile
{
    public float spreadAngle = 15f;

    Rigidbody2D rb;
    float timer = 0f;

    IAimer currAimer;

    public float Range { get; set; }
    public float Size { get; set; }
    public float Speed { get; set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currAimer = GetComponent<IAimer>();
    }

    private void OnEnable()
    {
        SetFireAngle();
        transform.localScale = new Vector3(Size, Size);
    }

    private void SetFireAngle()
    {
        Vector2 pos = currAimer.AimDirection();
        transform.up = pos;

        float baseAngle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;

        // 발사체가 2개 이상일 경우 상대 각도 계산
        if (currAimer.TotalCount > 1)
        {
            float totalSpreadAngle = spreadAngle * (currAimer.TotalCount - 1);
            float startAngle = baseAngle - totalSpreadAngle / 2f;
            float angleStep = totalSpreadAngle / (currAimer.TotalCount - 1);

            float currentAngle = startAngle + angleStep * currAimer.Index;
            float radianAngle = currentAngle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle));

            rb.velocity = direction * Speed;
        }
        else
        {
            rb.velocity = pos * Speed;
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
