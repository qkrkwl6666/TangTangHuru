using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public LayerMask attackableLayer;

    public float damage;
    public float pierceCount;

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnAttack(other);
    }

    public void OnAttack(Collider2D other)
    {
        if ((attackableLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            other.gameObject.GetComponent<IDamagable>().OnDamage(damage);

            if(pierceCount > 0)
            {
                pierceCount--;
            }
            else
            {
                gameObject.SetActive(false);
            }

        }
    }
}
