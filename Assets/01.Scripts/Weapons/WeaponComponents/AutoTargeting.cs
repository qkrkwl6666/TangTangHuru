using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTargeting : MonoBehaviour
{
    public LayerMask attackableLayer;
    private Rigidbody2D rb;

    public float damage;
    float lifeTime = 3f;
    float timer = 0f;
    int pierce = 0;


    public void Init(float damage, int pierce, Vector2 position, Vector2 dir)
    {
        gameObject.SetActive(true);
        this.damage = damage;
        this.pierce = pierce;
        transform.position = position;

        if (pierce > -1)
        {
            rb.velocity = dir * 15f;
        }
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


    private void OnTriggerEnter2D(Collider2D other)
    {
        OnAttack(other);
    }

    public void OnAttack(Collider2D other)
    {
        if ((attackableLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            other.gameObject.GetComponent<IDamagable>().OnDamage(damage);
            gameObject.SetActive(false);
        }
    }
}
