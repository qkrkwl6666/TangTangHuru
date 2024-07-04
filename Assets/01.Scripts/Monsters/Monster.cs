using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    MeleeAttackType,
    RangedAttackType,
    ChargeMeleeType,

    MonsterType1,
    MonsterType2,
    MonsterType3,
}

public class Monster : LivingEntity
{
    public float moveSpeed = 10f;
    public float attackDamage = 10f;

    public LivingEntity Player { get; private set; }

    private void Awake()
    {
        var go = GameObject.FindWithTag("Player");

        if (go == null) return;

        Player = go.GetComponent<LivingEntity>();
    }

    private void Update()
    {
        
    }
}
