using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public Slider hpBar;

    private float invincibleTime = 0.1f;
    private float timer = 0f;
    private bool isInvincible = false;

    void Start()
    {
        hpBar.maxValue = startingHealth;
        hpBar.value = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(isInvincible)
        {
            timer += Time.deltaTime;

            if(timer > invincibleTime)
            {
                isInvincible = false;
            }
        }
    }

    public override void OnDamage(float damage)
    {
        if (dead || isInvincible)
            return;

        health -= damage;
        hpBar.value -= damage;

        if (health <= 0 && !dead)
        {
            Die();
        }

        isInvincible = true;
    }
}
