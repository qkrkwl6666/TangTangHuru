using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public Slider hpBar;

    private float invincibleTime = 0.1f;
    private float timer = 0f;
    private bool isInvincible = false;

    private float armorRate = 0f;
    private float dodgeRate = 60f;

    void Start()
    {
        hpBar.maxValue = startingHealth;
        hpBar.value = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInvincible)
        {
            timer += Time.deltaTime;

            if (timer > invincibleTime)
            {
                isInvincible = false;
            }
        }
    }

    public void SetInfo(float hp, float def, float dodge)
    {
        health += hp;
        armorRate += def;
        dodgeRate += dodge;
    }

    public override void OnDamage(float damage, float Impact = 0)
    {
        if (dead || isInvincible)
            return;

        if (dodgeRate >= Random.Range(0f, 100f))
        {
            //미스효과 띄우기
            Debug.Log("빗나감!");
            return;
        }

        var totalDmg = damage - armorRate;
        if(totalDmg <= 0 )
        {
            //공식 변경 가능
            totalDmg = 1;
        }

        health -= totalDmg;
        hpBar.value -= damage;

        if (health <= 0 && !dead)
        {
            Die();
        }

        isInvincible = true;

        Vibration.Vibrate(0.1f);
    }


    public override void Die()
    {
        if (onDeath != null)
        {
            onDeath();
        }

        StopAllCoroutines();
        dead = true;
    }
}
