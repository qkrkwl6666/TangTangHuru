using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadWall : MonoBehaviour
{
    public float spreadAngle = 15f;

    float timer = 0f;
    float range = 3f;

    IAimer currAimer;
    Vector2 direction = Vector2.zero;

    void Awake()
    {
        currAimer = GetComponent<IAimer>();
    }

    private void OnEnable()
    {
        SetAngle();
    }

    private void SetAngle()
    {
        Vector2 pos = currAimer.AimDirection();

        float baseAngle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;

        // �߻�ü�� 2�� �̻��� ��� ��� ���� ���
        if (currAimer.TotalCount > 1)
        {
            float totalSpreadAngle = spreadAngle * (currAimer.TotalCount - 1);
            float startAngle = baseAngle - totalSpreadAngle / 2f;
            float angleStep = totalSpreadAngle / (currAimer.TotalCount - 1);

            float currentAngle = startAngle + angleStep * currAimer.Index;
            float radianAngle = currentAngle * Mathf.Deg2Rad;
            direction = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle));

            transform.position = direction * range;
            transform.up = direction;
        }
        else
        {
            transform.position = pos * range;
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
            transform.position = currAimer.Player.transform.position + (Vector3)direction * range;
            transform.up = direction;

            timer += Time.deltaTime;
        }
    }
}
