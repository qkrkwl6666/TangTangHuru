using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamagable
{
    public float startingHealth = 100f; // 시작 체력
    public float health { get; protected set; } // 현재 체력
    public bool dead { get; protected set; } // 사망 상태

    public Action<float> OnDamaged; //대미지 받았을때 이벤트

    public Action onDeath; // 사망시 발동할 이벤트

    protected virtual void OnEnable()
    {
        dead = false;
        health = startingHealth;
    }

    public void ApplyUpdateHealth(float newhealth, bool newDead)
    {
        health = newhealth;
        dead = newDead;
    }

    public virtual void OnDamage(float damage)
    {
        //Debug.Log("아야! hp : " + health);

        health -= damage;

        OnDamaged?.Invoke(damage);

        if (health <= 0 && !dead)
        {
            Die();
        }
    }
    public virtual void RestoreHealth(float newHealth)
    {
        if (dead)
        {
            return;
        }

        health += newHealth;
    }

    public virtual void Die()
    {
        if (onDeath != null)
        {
            onDeath();
        }

        dead = true;
    }

}
