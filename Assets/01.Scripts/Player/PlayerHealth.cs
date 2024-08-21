using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public Slider hpBar;
    public ParticleSystem healPartice;

    private float invincibleTime = 0.07f;
    private float timer = 0f;
    private bool isInvincible = false;

    private float armorRate = 1f;
    private float dodgeRate = 1f;

    private GameObject textObject;
    private List<GameObject> textObjects = new List<GameObject>();

    void Start()
    {
        hpBar.maxValue = startingHealth;
        hpBar.value = startingHealth;

        Addressables.LoadAssetAsync<GameObject>("DamageText").Completed += OnDamageTextLoaded;
    }

    private void OnDamageTextLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            textObject = handle.Result;
        }
        else
        {
            Debug.LogError("Failed to load DamageText prefab.");
        }
    }

    public void ShowMiss(Vector3 targetPos)
    {
        if (textObjects.Count < 100)
        {
            var newText = Instantiate(textObject, targetPos, Quaternion.identity);
            newText.GetComponent<TextMeshPro>().text = "Miss!";
            newText.GetComponent<TextMeshPro>().color = Color.white;
            textObjects.Add(newText);
            return;
        }

        foreach (var text in textObjects)
        {
            if (!text.activeSelf)
            {
                text.SetActive(true);
                text.transform.position = targetPos;
                text.GetComponent<TextMeshPro>().text = "Miss!";
                return;
            }
        }
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
        if (damage < 0)
        {
            if (health > startingHealth)
            {
                health = startingHealth;
                hpBar.value = startingHealth;
            }
            else
            {
                health -= damage;
                hpBar.value -= damage;
            }

            if (!healPartice.gameObject.activeSelf)
            {
                StartCoroutine(HealEffectPlay());
            }
            return;
        }

        if (dead || isInvincible)
            return;

        if (dodgeRate >= Random.Range(0f, 100f))
        {
            var showPos = transform.position;
            showPos.y += 1f;
            //미스효과 띄우기
            ShowMiss(showPos);
            return;
        }

        float totalDmg = damage * (damage / (damage + armorRate));

        if (totalDmg <= 1 )
        {
            totalDmg = 1;
        }

        health -= totalDmg;
        hpBar.value -= totalDmg;

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

    public void HealthCheatOn()
    {
        isInvincible = true;
        invincibleTime = 999999f;
    }

    public void HealthCheatOff()
    {
        isInvincible = false;
        invincibleTime = 0.1f;
    }


    private IEnumerator HealEffectPlay()
    {
        healPartice.gameObject.SetActive(true);
        healPartice.Play();
        yield return new WaitForSeconds(1f);
        healPartice.Stop();
        healPartice.gameObject.SetActive(false);
    }
}
