using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

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
    public int Gold { get; private set; }
    public float TotalCooldown { get; private set; }
    public float Range { get; private set; }

    public float AttackInterval { get; private set; } = 1f; // 공격 주기

    public PlayerSubject playerSubject;
    private Transform playerTransform;

    public Slider hpBar;
    private bool isSliderVisible = true;

    public void Initialize(PlayerSubject playerSubject)
    {
        this.playerSubject = playerSubject;
    }
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
        AttackInterval = monsterData.AttackInterval;

        playerSubject.AddObserver(this);

        AwakeHealth();
    }

    public void SetHpBar()
    {
        hpBar.maxValue = startingHealth;
        hpBar.value = startingHealth;

        hpBar.gameObject.SetActive(false);
    }

    private IObjectPool<GameObject> pool;

    public void SetPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }

    // 몬스터 죽을 떄 호출 해야함
    public void PoolRelease()
    {
        if (pool == null) Destroy(gameObject);

        pool.Release(gameObject);
    }

    // 오버라이드 LivingEntity event -> delegate 로 변경햇습니다
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

        PoolRelease();
    }

    public void IObserverUpdate()
    {
        playerTransform = playerSubject.GetPlayerTransform;
    }

    public override void OnDamage(float damage, float impact)
    {
        health -= damage;

        OnDamaged?.Invoke(damage);
        OnImpact?.Invoke(impact);

        if (health <= 0 && !dead)
        {
            Die();
        }

        if (isSliderVisible)
        {
            if (!hpBar.gameObject.activeSelf)
            {
                hpBar.gameObject.SetActive(true);
            }
            hpBar.value = health;
        }
    }

    public void ActiveHpSlider(bool isVisible)
    {
        if (isVisible)
        {
            isSliderVisible = true;
        }
        else
        {
            isSliderVisible = false;
            hpBar.gameObject.SetActive(false);
        }
    }
}
