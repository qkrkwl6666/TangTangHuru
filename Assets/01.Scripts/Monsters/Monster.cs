using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    MeleeAttackType,
    RangedAttackType,
    ChargeMeleeType,

    MonsterType1 = 100001,
    MonsterType2,
    MonsterType3,
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
