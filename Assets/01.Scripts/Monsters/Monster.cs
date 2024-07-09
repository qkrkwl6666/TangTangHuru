using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
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

    public void SetPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }

    // ���� ���� �� ȣ�� �ؾ���
    public void PoolRelease()
    {
        if(pool == null) Destroy(gameObject);

        pool.Release(gameObject);
    }

    // �������̵� LivingEntity event -> delegate �� �����޽��ϴ�
    public override void Die()
    {
        if (onDeath != null)
        {
            onDeath();
        }

        Addressables.InstantiateAsync(Defines.exp).Completed += (x) => 
        {
            //x.Result.GetComponent<MonsterExp>().Initialize(playerSubject)
        };

        dead = true;

        PoolRelease();

        
    }


}
