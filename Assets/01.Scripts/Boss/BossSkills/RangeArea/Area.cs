using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Area : MonoBehaviour
{
    private IObjectPool<GameObject> pool;

    private float scale = 1f;
    private float duration = 1.5f;
    private float time = 0f;

    public void InitSkill(float scale, float duration)
    {
        scale = this.scale;
        this.duration = duration;
    }

    private void Update()
    {
        time += Time.deltaTime;

        float currentScale = Mathf.InverseLerp(0f, duration, time) * scale;

        transform.localScale = new Vector3 (currentScale, currentScale, currentScale);

        if (time >= duration) 
        {
            pool?.Release(gameObject);
        }
    }

    public void SetObjectPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }

}
