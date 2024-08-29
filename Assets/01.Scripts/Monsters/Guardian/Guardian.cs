using UnityEngine;

// 안쓰는 스크립트

public class Guardian : LivingEntity, IPlayerObserver
{
    public float MoveSpeed { get; private set; }
    public float Damage { get; private set; }
    public float Exp { get; private set; }
    public int Gold { get; private set; }
    public float IdleCooldown { get; private set; }
    public float Range { get; private set; }

    public float AttackInterval { get; private set; } = 1f; // 공격 주기

    public PlayerSubject playerSubject;
    private Transform playerTransform;

    public void Initialize(PlayerSubject playerSubject)
    {
        this.playerSubject = playerSubject;
    }

    public void IObserverUpdate()
    {
        playerTransform = playerSubject.GetPlayerTransform;
    }

    public void Initialize(PlayerSubject playerSubject, in MonsterData monsterData)
    {
        this.playerSubject = playerSubject;

        if (playerSubject == null)
        {
            //Debug.Log("MonsterExp Script PlayerSubject is Null");
            return;
        }

        startingHealth = monsterData.Monster_Hp;
        MoveSpeed = monsterData.Monster_MoveSpeed;
        Damage = monsterData.Monster_Damage;
        Exp = monsterData.Monster_Exp;
        Gold = monsterData.Monster_Gold;
        IdleCooldown = monsterData.Cooldown;
        Range = monsterData.Range;
        AttackInterval = monsterData.AttackInterval;

        playerSubject.AddObserver(this);

        AwakeHealth();
    }

    public override void Die()
    {
        if (onDeath != null)
        {
            onDeath();
        }

        InGameInventory.OnCoinAdd?.Invoke((int)Gold);
        InGameInventory.OnKillAdd?.Invoke();

        var go = ObjectPoolManager.expPool.Get();
        go.GetComponent<MonsterExp>().Initialize(playerSubject, transform, Exp);

        dead = true;
    }


}
