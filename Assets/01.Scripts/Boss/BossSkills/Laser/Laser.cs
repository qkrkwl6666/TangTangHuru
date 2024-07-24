using UnityEngine;
using UnityEngine.Pool;
using static LaserSkill;

public class Laser : MonoBehaviour
{
    public LaserSkill.LaserSetting laserSetting = new LaserSkill.LaserSetting();

    private IObjectPool<GameObject> pool;

    private float damage = 0f;
    private float time = 0f;
    private bool isRatation = false;
    private float rotationTime = 0f;
    private Transform bossTransform;
    private float currentAngle = 0f;

    private float initialAngle = 0f;

    private float attackDuration = 0.05f;
    private float attackTime = 0f;
    private bool attackable = false;

    private LaserSkill.RotationType rotationType;
    private int currentIndex = 0;

    public void Init()
    {
        currentIndex = 0;
        rotationType = laserSetting.rotationTypes[currentIndex];
    }

    public void SetDamageTransform(float damage, Transform bossTransform)
    {
        this.damage = damage;
        this.bossTransform = bossTransform;
    }

    public void SetObjectPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (!attackable)
        {
            attackTime += Time.deltaTime;

            if (attackTime >= attackDuration)
            {
                attackable = true;
                attackTime = 0f;
            }
        }

        if (time < laserSetting.laserRate && !isRatation)
        {
            float yScale = Mathf.InverseLerp(0f, laserSetting.laserRate, time)
                * laserSetting.yScale;

            transform.localScale = new Vector3(1f, yScale, 1f);
        }

        if (time >= laserSetting.laserRate && !isRatation)
        {
            isRatation = true;
            currentAngle = initialAngle;
        }

        if (isRatation && rotationTime >= laserSetting.rotationTime)
        {
            if (currentIndex + 1 == laserSetting.rotationTypes.Count)
            {
                pool.Release(gameObject);
                return;
            }
            currentIndex++;
            rotationType = laserSetting.rotationTypes[currentIndex];
            rotationTime = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (isRatation)
        {
            LaserRotation(Time.fixedDeltaTime);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!attackable) return;

        OnAttack(collision);
    }

    public void LaserRotation(float fixedDeltaTime)
    {
        rotationTime += fixedDeltaTime;
        switch (rotationType)
        {
            case RotationType.Right:
                currentAngle += fixedDeltaTime * laserSetting.rotationSpeed;
                break;
            case RotationType.Left:
                currentAngle -= fixedDeltaTime * laserSetting.rotationSpeed;
                break;
        }

        var dreangle = currentAngle * Mathf.Deg2Rad;

        Vector2 nextDir = new Vector2(Mathf.Cos(dreangle), Mathf.Sin(dreangle))
            .normalized * laserSetting.laserDistance;

        // 보스를 향해 바라보도록 회전
        Vector2 directionToBoss = (bossTransform.position - transform.position).normalized;
        float angle = Mathf.Atan2(directionToBoss.y, directionToBoss.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        transform.position = (Vector2)bossTransform.position + nextDir;
    }

    public void SetInitialAngle(float angle)
    {
        initialAngle = angle;
    }

    private void OnDisable()
    {
        transform.localScale = new Vector3(1f, 0f, 1f);
        time = 0f;
        isRatation = false;
        rotationTime = 0f;
    }

    public void OnAttack(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<IDamagable>().OnDamage(damage, 0);
            attackable = false;
        }
    }
}
