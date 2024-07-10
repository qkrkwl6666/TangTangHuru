using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Hit : MonoBehaviour
{
    public LayerMask attackableLayer;

    public float damage;
    public float pierceCount;

    public float criticalChance;
    public float criticalValue;

    private float totalDamage;

    private bool attackable = true;
    private float attackRate = 0.1f;
    private float timer = 0f;

    private void Update()
    {
        if(timer > attackRate)
        {
            attackable = true;
            timer = 0f;
        }
        else
        {
            timer += Time.deltaTime;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!attackable)
            return;

        OnAttack(other);
    }

    public void OnAttack(Collider2D other)
    {
        var pierce = pierceCount;

        if ((attackableLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            if(Random.Range(0, 100) <= criticalChance)
            {
                totalDamage = damage * criticalValue;
            }
            else
            {
                totalDamage = damage;
            }

            other.gameObject.GetComponent<IDamagable>().OnDamage(totalDamage);

            if(pierce > 0)
            {
                pierce--;
            }
            else
            {
                gameObject.SetActive(false);
            }

            attackable = false;
        }
    }

}
