using System.Collections;
using System.Collections.Generic;
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
