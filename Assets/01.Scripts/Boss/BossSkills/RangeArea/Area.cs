using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Area : MonoBehaviour
{
    private IObjectPool<GameObject> pool;

    private float damage = 0f;
    private float scale = 1f;
    private float duration = 1.5f;
    private float time = 0f;
    private bool attackable = false;

    private Transform playerTransform;

    public void InitSkill(float scale, float duration, Transform playerTransform)
    {
        this.scale = scale;
        this.duration = duration;
        this.playerTransform = playerTransform;

        Debug.Log($"Scale : {this.scale} Duration : {this.duration}");
    }

    private void OnDisable()
    {
        time = 0f;
        attackable = false;
    }

    private void Update()
    {
        time += Time.deltaTime;

        float currentScale = Mathf.InverseLerp(0f, duration, time) * scale;

        transform.localScale = new Vector3 (currentScale, currentScale, currentScale);

        if (time >= duration) 
        {
            if(playerTransform != null)
            {
                float Distance = Vector2.Distance(transform.position, playerTransform.position);
                if(Distance <= scale / 2)
                {
                    playerTransform.GetComponent<IDamagable>().OnDamage(damage, 0);
                }
            }
            pool?.Release(gameObject);
        }
    }

    public void SetObjectPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;

    }

}

