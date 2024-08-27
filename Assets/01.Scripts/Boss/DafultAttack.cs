using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DafultAttack : MonoBehaviour
{
    private float attackInterval = 1f;
    private float time = 0f;
    private float damage;
    private float rangeAttack = 1;

    public Transform playerTransform;

    public void Init(Transform playerTransform, float attackInterval, float damage, float rangeAttack)
    {
        this.playerTransform = playerTransform;
        this.attackInterval = attackInterval;
        this.damage = damage;
        this.rangeAttack = rangeAttack;
    }

    private void Update()
    {
        if (playerTransform == null) return;

        time += Time.deltaTime;

        float dis = Vector2.Distance(playerTransform.position, transform.position);

        if (time >= attackInterval && dis <= rangeAttack)
        {
            time = 0f;
            playerTransform.GetComponent<IDamagable>().OnDamage(damage, 0);
        }
    }
}
