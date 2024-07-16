using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public IAimer currAimer;

    private float timer = 0f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currAimer = GetComponent<IAimer>();
    }
    private void Start()
    {

    }

    private void OnEnable()
    {
        var pos = currAimer.AimDirection();

        if (pos == Vector3.zero)
        {
            pos = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        }

        transform.up = pos;
        rb.velocity = pos * currAimer.Speed;

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

