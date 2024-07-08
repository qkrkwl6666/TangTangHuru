using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    MeleeAttackType = 1,
    RangedAttackType,
    ChargeMeleeType,
}

public class Monster : LivingEntity
{
    public float moveSpeed = 10f;
    public float attackDamage = 10f;

    private void Awake()
    {

    }
    private void Update()
    {
        
    }

}
