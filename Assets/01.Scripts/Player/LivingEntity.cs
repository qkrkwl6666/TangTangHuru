using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamagable
{
    public float startingHealth = 100f; // 시작 체력
    public float health { get; protected set; } // 현재 체력
    public bool dead { get; protected set; } // 사망 상태

    public Action<float> OnDamaged; //대미지 받았을때 이벤트
    public Action<float> OnImpact; //넉백 받았을때 이벤트

    public Action onDeath; // 사망시 발동할 이벤트

    protected virtual void OnEnable()
    {
        // Todo : 활성화 주기랑 startingHealth 초기화 안 맞을 가능성 있음
        dead = false;
        health = startingHealth;
    }

    public void AwakeHealth()
    {
        dead = false;
        health = startingHealth;
    }

    public void ApplyUpdateHealth(float newhealth, bool newDead)
    {
        health = newhealth;
        dead = newDead;
    }

    public virtual void OnDamage(float damage, float impact)
    {
        //Debug.Log("아야! hp : " + health);

        health -= damage;

        OnDamaged?.Invoke(damage);
        OnImpact?.Invoke(impact);

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
