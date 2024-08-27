using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Monster : LivingEntity, IPlayerObserver
{
    private IObjectPool<GameObject> pool;

    public float MoveSpeed { get; private set; }

    public float Damage { get; private set; }
    public float Exp { get; private set; }
    public int Gold { get; private set; }
    public float TotalCooldown { get; private set; }
    public float Range { get; private set; }

    public float AttackInterval { get; private set; } = 1f; // ���� �ֱ�

    public PlayerSubject playerSubject;
    private Transform playerTransform;

    public Slider hpBar;
    private bool isSliderVisible = true;
    public bool isBoomType = false;

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

    public void SetPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }

    // ���� ���� �� ȣ�� �ؾ���
    public void PoolRelease()
    {
        if (pool == null)
        {
            Destroy(gameObject);
            return;
        }

        pool.Release(gameObject);
    }

    // �������̵� LivingEntity event -> delegate �� �����޽��ϴ�
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

        if(isBoomType)
        {
            var boom = ObjectPoolManager.boomPool.Get();
            boom.transform.position = transform.position;
        }

        dead = true;

        PoolRelease();

        DOTween.Kill(transform);
    }

    public void IObserverUpdate()
    {
        playerTransform = playerSubject.GetPlayerTransform;
    }

    public override void OnDamage(float damage, float impact = 0)
    {
        NuckBack(impact);

        health -= damage;

        OnDamaged?.Invoke(damage);
        //OnImpact?.Invoke(impact);

        if (health <= 0 && !dead)
        {
            Die();
        }

        if (hpBar == null) return;

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

    private void NuckBack(float impact)
    {
        //transform.Translate((gameObject.transform.position - PlayerTransform.position).normalized * impact);

        Vector3 direction = (transform.position - playerTransform.position).normalized;
        Vector3 targetPosition = transform.position + direction * impact;

        transform.DOMove(targetPosition, 0.3f).SetEase(Ease.OutQuad);
    }
}
