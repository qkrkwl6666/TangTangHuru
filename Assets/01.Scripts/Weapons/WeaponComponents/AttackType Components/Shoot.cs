using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public float speed = 0f;
    public float lifeTime = 0f;
    public IAimer currAimer;

    private float timer = 0f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        Vector3 targetPos;

        if (currAimer.AimDirection() != null)
        {
            targetPos = currAimer.AimDirection().position;
        }
        else
        {
            targetPos = Random.insideUnitCircle;
        }

        var dir = targetPos - transform.position;

        rb.velocity = dir.normalized * speed;
    }

    private void OnDisable()
    {
        timer = 0f;
    }

    private void Update()
    {
        if (timer > lifeTime)
        {
            gameObject.SetActive(false);
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}

