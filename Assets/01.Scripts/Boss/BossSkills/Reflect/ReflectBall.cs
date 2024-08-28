using UnityEngine;
using UnityEngine.Pool;

public class ReflectBall : MonoBehaviour
{
    private IObjectPool<GameObject> pool;

    private Transform bossTransform;
    private Vector2 dir = Vector2.zero;
    private float speed = 10f;
    private float circleScale = 1f;

    private float disableDuration = 8f;
    private float time = 0f;

    private float damage = 10f;

    private bool fission = false;
    private int copyCount = 0;

    private float maxAngle = 90f;

    public void Init(Vector2 dir, Transform bossTransform, float circleScale
        , float ballSpeed, float disableDuration, bool fission = false,
        float currentTime = 0f, int copyCount = 0)
    {
        this.dir = dir;
        this.bossTransform = bossTransform;
        this.circleScale = circleScale;
        this.speed = ballSpeed;
        this.fission = fission;
        this.time = currentTime;
        this.disableDuration = disableDuration;
        this.copyCount = copyCount;

        transform.localScale = new Vector3(circleScale, circleScale, circleScale);
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
    private void OnDisable()
    {
        copyCount = 0;
        fission = false;
        time = 0f;
        dir = Vector2.zero;
        speed = 10f;
        circleScale = 1f;
        disableDuration = 8f;
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (time >= disableDuration) 
        {
            pool.Release(gameObject);
        }

        transform.Translate(speed * Time.deltaTime * dir);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<IDamagable>().OnDamage(damage, 0);
            pool.Release(gameObject);
            return;
        }

        Fission(collision);
    }

    private void Fission(Collider2D collision)
    {
        if (fission && collision.gameObject.layer == LayerMask.NameToLayer(Defines.bossWall))
        {
            // 분열
            for (int i = 0; i < copyCount; i++)
            {
                // inDirection : dir
                // inNormal : 은 내가 가는 방향에 반대 방향
                // 현재 각도 의 반대 각도
                float defaultAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180;

                float angle = (maxAngle / copyCount * (i + 1)) + defaultAngle;
                int minus = copyCount / 2 + 1;
                float finalAngle = angle - (minus * (maxAngle / copyCount));

                Vector2 reflectBallDir = new Vector2(Mathf.Cos(finalAngle * Mathf.Deg2Rad),
                        Mathf.Sin(finalAngle * Mathf.Deg2Rad)).normalized;

                var ball = pool.Get().GetComponent<ReflectBall>();
                ball.transform.position = transform.position;

                ball.Init(reflectBallDir, bossTransform, circleScale / copyCount, speed, disableDuration, true, time);
            }

            pool.Release(gameObject);
            return;
        }
        else if (!fission && collision.gameObject.layer == LayerMask.NameToLayer(Defines.bossWall))
        {
            dir = Vector2.Reflect(dir.normalized, -dir.normalized);
        }
            
    }

    public void SetObjectPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }
}
