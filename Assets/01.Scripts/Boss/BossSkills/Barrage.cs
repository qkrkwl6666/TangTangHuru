using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Barrage : MonoBehaviour
{
    private IObjectPool<GameObject> pool;

    private float disableDuration = 8f;
    private float time = 0f;

    private void Update()
    {
        time += Time.deltaTime;

        if (time >= disableDuration)
        {
            time = 0f;
            pool.Release(gameObject);
        }
        
    }

    public void SetObjectPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool; 
    }
}
