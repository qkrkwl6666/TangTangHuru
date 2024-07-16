using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spread : MonoBehaviour
{
    public float spreadAngle = 15f;
    public int totalProjectiles = 0;
    public int projectileIndex = 0;

    Rigidbody2D rb;
    float timer = 0f;

    IAimer currAimer;
    Transform parentTransform;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currAimer = GetComponent<IAimer>();
        parentTransform = currAimer.Player.transform;

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

        // 발사체가 2개 이상일 경우 상대 각도 계산
        if (totalProjectiles > 1)
        {
            float totalSpreadAngle = spreadAngle * (totalProjectiles - 1);
            float startAngle = baseAngle - totalSpreadAngle / 2f;
            float angleStep = totalSpreadAngle / (totalProjectiles - 1);

            float currentAngle = startAngle + angleStep * projectileIndex;
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
