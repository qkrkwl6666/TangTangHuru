using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    RangeDetecter detector;

    private Rigidbody2D rb;

    public float damage;
    float lifeTime = 3f;
    float timer = 0f;
    int pierce = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        detector = new RangeDetecter();
    }

    private void OnEnable()
    {
        timer = 0f;
    }

    private void Update()
    {
        if (timer > lifeTime)
        {
            gameObject.SetActive(false);
            timer = 0f;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}

