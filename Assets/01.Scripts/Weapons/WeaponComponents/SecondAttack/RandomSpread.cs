using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpread : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed;
    private float timer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = 15f;
    }

    private void OnEnable()
    {
        timer = 0f;
        var rnd = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));
        transform.up = rnd.normalized;
        rb.velocity = rnd.normalized * speed;
    }

    void Update()
    {
        if (timer > 1.5f)
        {
            gameObject.SetActive(false);
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}
