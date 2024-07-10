using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamagable
{
    public float startingHealth = 100f; // ���� ü��
    public float health { get; protected set; } // ���� ü��
    public bool dead { get; protected set; } // ��� ����

    public Action<float> OnDamaged; //����� �޾����� �̺�Ʈ

    public Action onDeath; // ����� �ߵ��� �̺�Ʈ

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
        //Debug.Log("�ƾ�! hp : " + health);

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
