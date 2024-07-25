using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    public float lifeTime = 2f;
    float timer = 0;

    private void OnEnable()
    {
        timer = 0;
    }

    private void Update()
    {
        if (timer > lifeTime)
        {
            gameObject.SetActive(false);
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}
