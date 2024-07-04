using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public LayerMask attackableLayer;

    public WeaponInfo weaponInfo;

    private float damage;

    void Start()
    {
        weaponInfo.GetComponent<WeaponInfo>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnAttack(other);
    }

    public void OnAttack(Collider2D other)
    {
        if ((attackableLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            other.gameObject.GetComponent<IDamagable>().OnDamage(damage);
            gameObject.SetActive(false);
        }
    }
}
