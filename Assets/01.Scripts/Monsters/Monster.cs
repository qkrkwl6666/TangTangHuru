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
public class Monster : LivingEntity, IPlayerObserver
{
    public float MoveSpeed { get; private set; }

    public float Damage { get; private set; }
    public float Exp { get; private set; }
    public float Gold { get; private set; }
    public float TotalCooldown { get; private set; }
    public float Range { get; private set; }

    public PlayerSubject playerSubject;
    private Transform playerTransform;
   
    public void Initialize(PlayerSubject playerSubject, in MonsterData monsterData)
    {
        this.playerSubject = playerSubject;

        if (playerSubject == null)
        {
            Debug.Log("MonsterExp Script PlayerSubject is Null");
            return;
        }

        startingHealth = monsterData.Monster_Hp;
        MoveSpeed = monsterData.Monster_MoveSpeed;
        Damage = monsterData.Monster_Damage;
        Exp = monsterData.Monster_Exp;
        Gold = monsterData.Monster_Gold;
        TotalCooldown = monsterData.Cooldown;
        Range = monsterData.Range;

        playerSubject.AddObserver(this);
    }

    private IObjectPool<GameObject> pool;

    public void SetPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }

    // 몬스터 죽을 떄 호출 해야함
    public void PoolRelease()
    {
        if(pool == null) Destroy(gameObject);

        pool.Release(gameObject);
    }

    // 오버라이드 LivingEntity event -> delegate 로 변경햇습니다
    public override void Die()
    {
        if (onDeath != null)
        {
            onDeath();
        }

        var go = ObjectPoolManager.expPool.Get();
        go.GetComponent<MonsterExp>().Initialize(playerSubject, transform, Exp);
        dead = true;

        PoolRelease();
    }

    public void IObserverUpdate()
    {
        playerTransform = playerSubject.GetPlayerTransform;
    }
}
