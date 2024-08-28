using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Tornado : MonoBehaviour
{
    private IObjectPool<GameObject> pool;

    private Transform bossTransform;
    private Vector2 dir = Vector2.zero;
    private float speed = 10f;
    private float disableDuration = 10f;

    private float time = 0f;

    private float damage = 10f;

    private void OnDisable()
    {
        time = 0;
        dir = Vector2.zero;
    }

    public void Init(Vector2 dir, float disableDuration, float speed, float scale = 1f)
    {
        this.dir = dir;
        this.disableDuration = disableDuration;
        this.speed = speed;

        transform.localScale = new Vector3 (scale, scale, scale);
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    private void Update()
    {
        time += Time.deltaTime;

        transform.Translate(speed * Time.deltaTime * dir);

        if(time >= disableDuration)
        {
            pool.Release(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnAttack(collision);
    }

    public void OnAttack(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<IDamagable>().OnDamage(damage, 0);
            pool.Release(gameObject);
        }
    }

    public void SetObjectPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }
}
