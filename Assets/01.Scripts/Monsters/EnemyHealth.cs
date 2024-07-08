using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : LivingEntity
{
    public Slider hpBar;

    void Start()
    {
        hpBar.maxValue = startingHealth;
        hpBar.value = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnDamage(float damage)
    {

        Debug.Log("¾Æ¾ß! hp : " + health);

        health -= damage;
        hpBar.value -= damage;

        if (health <= 0 && !dead)
        {
            Die();
        }
    }
}
