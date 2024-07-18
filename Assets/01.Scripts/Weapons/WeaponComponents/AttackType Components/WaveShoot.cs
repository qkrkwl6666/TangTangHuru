using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveShoot : MonoBehaviour
{
    public IAimer currAimer;

    private float timer = 0f;
    private Rigidbody2D rb;

    //
    private Vector2 initialPosition;
    private Vector2 targetPosition;

    private float frequency = 15f; // �ⷷ�̴� ��
    private float magnitude = 2f; // �ⷷ�̴� ����
    //

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currAimer = GetComponent<IAimer>();
    }

    private void OnEnable()
    {
        timer = 0f;
        var pos = currAimer.AimDirection();
        transform.up = pos;
        rb.velocity = pos * currAimer.Speed;

        initialPosition = transform.position;
        targetPosition = initialPosition + (Vector2)pos * currAimer.Speed * currAimer.LifeTime;

        frequency *= (currAimer.Index % 2) == 0 ? -1f : 1f;
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
        float sineOffset = Mathf.Sin(timer * frequency) * magnitude;
        Vector2 perpendicular = Vector2.Perpendicular((targetPosition - initialPosition).normalized);
        Vector2 sineWavePosition = linearPosition + perpendicular * sineOffset;

        transform.position = sineWavePosition;
    }
}
