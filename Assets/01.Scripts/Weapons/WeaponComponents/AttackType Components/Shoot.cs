using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public WeaponInfo weaponInfo;
    public MonoBehaviour aimer;
    private IAimer currAimer;

    private Rigidbody2D rb;
    private float timer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currAimer = aimer as IAimer;
    }

    private void OnEnable()
    {
        Vector3 targetPos;

        if (currAimer.AimDirection() != null)
        {
            targetPos = currAimer.AimDirection().position;
        }
        else
        {
            targetPos = Random.insideUnitCircle;
        }

        var dir = targetPos - transform.position;

        rb.velocity = dir.normalized * weaponInfo.weapon_Speed;
    }

    private void OnDisable()
    {
        timer = 0f;
    }

    private void Update()
    {
        if (timer > weaponInfo.weapon_LifeTime)
        {
            gameObject.SetActive(false);
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}

