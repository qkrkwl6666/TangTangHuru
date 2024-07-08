using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

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

    private IObjectPool<GameObject> pool;

    private void Awake()
    {

    }
    private void Update()
    {
        
    }

    public void SetPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }

    public void PoolRelease()
    {
        pool.Release(this.gameObject);
    }

}
