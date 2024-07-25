using UnityEngine;

public class SpreadWall : MonoBehaviour, IProjectile
{
    public float spreadAngle = 15f;

    float timer = 0f;

    IAimer currAimer;
    Vector2 direction = Vector2.zero;

    public float Range { get; set; }
    public float Size { get; set; }
    public float Speed { get; set; }

    void Awake()
    {
        currAimer = GetComponent<IAimer>();
    }

    private void OnEnable()
    {
        SetAngle();
        transform.localScale = new Vector3(Size, Size);
    }

    private void SetAngle()
    {
        Vector2 pos = currAimer.AimDirection();

        float baseAngle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;

        // 발사체가 2개 이상일 경우 상대 각도 계산
        if (currAimer.TotalCount > 1)
        {
            float totalSpreadAngle = spreadAngle * (currAimer.TotalCount - 1);
            float startAngle = baseAngle - totalSpreadAngle / 2f;
            float angleStep = totalSpreadAngle / (currAimer.TotalCount - 1);

            float currentAngle = startAngle + angleStep * currAimer.Index;
            float radianAngle = currentAngle * Mathf.Deg2Rad;
            direction = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle));

            transform.position = direction * Range;
            transform.up = direction;
        }
        else
        {
            transform.position = pos * Range;
            transform.up = pos;
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
            transform.position = currAimer.Player.transform.position + (Vector3)direction * Range;
            transform.up = direction;

            timer += Time.deltaTime;
        }
    }
}
