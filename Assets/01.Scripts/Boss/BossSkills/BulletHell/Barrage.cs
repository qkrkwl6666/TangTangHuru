using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class Barrage : MonoBehaviour
{
    private IObjectPool<GameObject> pool;

    private Transform bossTransform;
    private Vector2 dir = Vector2.zero;
    private float speed = 10f;
    private float circleScale = 1f;

    private float disableDuration = 8f;
    private float time = 0f;

    private float damage = 10f;

    private float atkRate = 3f;
    private bool attackable = false;
    private float attackTime = 3f;

    public LayerMask attackableLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!attackable) return;
        
        OnAttack(collision);
    }

    public void OnAttack(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<IDamagable>().OnDamage(damage);
        }
    }

    public void Init(Vector2 dir, Transform bossTransform, float attackCicle)
    {
        this.dir = dir.normalized;
        this.bossTransform = bossTransform;
        circleScale = attackCicle;
        transform.localScale = new Vector3(circleScale, circleScale, circleScale);

        transform.position = (Vector2)bossTransform.position + dir;
    }

    private void Update()
    {
        if (bossTransform == null) pool.Release(gameObject);

        time += Time.deltaTime;
        attackTime += Time.deltaTime;

        if(attackTime >= atkRate && !attackable)
        {
            attackTime = 0f;
            attackable = true;
        }

        if (time >= disableDuration)
        {
            time = 0f;
            pool.Release(gameObject);
        }

        transform.Translate(dir * speed * Time.deltaTime);
    }

    public void SetObjectPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool; 
    }
}
