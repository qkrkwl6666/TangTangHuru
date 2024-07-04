using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyChase : MonoBehaviour
{
    private GameObject target;
    private Rigidbody2D rb;

    public float speed = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Chase();
    }

    void Chase()
    {
        Vector2 dir;

        if (target != null)
        {
            dir = target.transform.position - transform.position;
        }
        else
        {
            dir = transform.position;
        }


        rb.velocity = dir.normalized * speed;
    }
}
