using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask attackableLayer;
    public float damage;
    float lifeTime = 3f;
    float timer = 0f;
    private void Update()
    {
        if(timer > lifeTime)
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
        if(other.gameObject.layer == attackableLayer)
        {
        }

    }
}
