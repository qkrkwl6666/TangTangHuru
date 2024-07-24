using System;
using System.Collections.Generic;
using UnityEngine;

public class Boss : LivingEntity, IPlayerObserver
{
    private List<(IBossSkill skill, float probability)> skills = new List<(IBossSkill, float)>();
    private IBossSkill currentSkill = null;

    private PlayerSubject playerSubject;
    public Transform PlayerTransform { get; private set; }

    private float totalProbability = 0f;
    public float Cooldown { get; private set; } = 2f;
    private float time = 0f;

    private float damage = 0f;
    public float Speed { get; private set; } = 2f;

    public static Action OnDead;

    // 골드
    public int Gold { get; private set; }

    // 보스 상태 패턴
    private BossState currentState;

    public BossWalkState walkState;
    public BossSkillState skillState;
    public BossDeadState deadState;

    // View
    private MonsterView monsterView;

    public InGameUI GameUI { get; private set; }

    private void Awake()
    {
        monsterView = GetComponentInChildren<MonsterView>();
        GameUI = GameObject.FindWithTag("InGameUI").GetComponent<InGameUI>();
    }

    public void Initialize(PlayerSubject playerSubject, BossData bossData)
    {
        this.playerSubject = playerSubject;

        playerSubject.AddObserver(this);

        startingHealth = bossData.Boss_Hp;
        damage = bossData.Boss_Damage;
        Cooldown = bossData.Boss_Cooldown;
        Speed = bossData.Boss_MoveSpeed;
        Gold = bossData.Gold;

        walkState = new BossWalkState(this, monsterView);
        skillState = new BossSkillState(this, monsterView);
        deadState = new BossDeadState(this, monsterView);

        SetBossSkill(bossData);

        AwakeHealth();
    }
    public void SetBossSkill(BossData bossData)
    {
        var skillList = bossData.GetBossSkillId();

        foreach (var skill in skillList)
        {
            if (skill.Item1 == -1) break;

            switch (skill.Item1)
            {
                case 500001:
                    {
                        var bn = AddSkill<BarrageNormal>(skill.Item1, skill.Item2);

                        bn.SetCountScale(10, 0.1f);
                    }
                    break;
                case 500002:
                    {
                        var bn = AddSkill<BarrageNormal>(skill.Item1, skill.Item2);

                        bn.SetCountScale(20, 0.07f);
                    }
                    break;
                case 500003:
                    {
                        var bn = AddSkill<BarrageSnail>(skill.Item1, skill.Item2);
                    }
                    break;
                case 500004:
                    {
                        var bn = AddSkill<Rush>(skill.Item1, skill.Item2);
                    }
                    break;
                case 500007:
                    {
                        var bn = AddSkill<RangeArea>(skill.Item1, skill.Item2);
                        bn.SetScaleDuration(3f, 6f);
                    }
                    break;
                case 500008:
                    {
                        var bn = AddSkill<RangeArea>(skill.Item1, skill.Item2);
                        bn.SetScaleDuration(2f, 4.5f);
                    }
                    break;
                case 500009:
                    {
                        var bn = AddSkill<RangeArea>(skill.Item1, skill.Item2);
                        bn.SetScaleDuration(1.5f, 2f);
                    }
                    break;
                case 500010:
                    {
                        var laser = AddSkill<LaserSkill>(skill.Item1, skill.Item2);
                        laser.SetLaser(1, 10f, 50f, 2f);
                        laser.laserSetting.rotationTypes.Add(LaserSkill.RotationType.Right);
                    }
                    break;
                case 500011:
                    {
                        var laser = AddSkill<LaserSkill>(skill.Item1, skill.Item2);
                        laser.SetLaser(4, 10f, 50f, 2f);
                        laser.laserSetting.rotationTypes.Add(LaserSkill.RotationType.Right);
                    }
                    break;
                case 500012:
                    {
                        var laser = AddSkill<LaserSkill>(skill.Item1, skill.Item2);
                        laser.SetLaser(4, 10f, 50f, 2f);
                        laser.laserSetting.rotationTypes.Add(LaserSkill.RotationType.Right);
                        laser.laserSetting.rotationTypes.Add(LaserSkill.RotationType.Left);
                    }
                    break;
            }
        }

        currentState = walkState;
        currentState.Enter();
    }

    private T AddSkill<T>(int skillId, float probability) where T : Component, IBossSkill
    {
        var bossSkill = gameObject.AddComponent<T>();
        var bossSkillData = DataTableManager.Instance.Get<BossSkillTable>
            (DataTableManager.bossSkill).Get(skillId.ToString());
        bossSkill.Initialize(bossSkillData, damage);
        skills.Add((bossSkill, probability));
        totalProbability += probability;

        return bossSkill;
    }

    public void Update()
    {
        if (currentState == null) return;

        currentState.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Vector2 dir = (PlayerTransform.position - gameObject.transform.position).normalized;

        monsterView.skeletonAnimation.skeleton.ScaleX = dir.x < 0 ? -1f : 1f;
    }

    public IBossSkill SelectSkill()
    {
        Debug.Log("SelectSkill");

        if (currentSkill != null)
        {
            currentSkill.DeActivate();
        }

        time = 0f;
        currentSkill = null;

        float random = UnityEngine.Random.Range(0, totalProbability);

        float currentProbability = 0f;

        foreach (var (skill, probability) in skills)
        {
            currentProbability += probability;

            if (random <= currentProbability)
            {
                skill.Activate();
                currentSkill = skill;
                break;
            }
        }

        return currentSkill;
    }

    public void ChangeState(BossState bossState)
    {
        currentState.Exit();
        currentState = bossState;
        currentState.Enter();
    }
    public override void Die()
    {
        base.Die();

        InGameInventory.OnCoinAdd?.Invoke(Gold);
        OnDead?.Invoke();

        ChangeState(deadState);
    }

    public void BossDestory()
    {
        Destroy(gameObject);
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    public override void OnDamage(float damage, float impact)
    {
        base.OnDamage(damage, 0);

        GameUI.UpdateBossHpBar(health / startingHealth);
    }

    public void IObserverUpdate()
    {
        PlayerTransform = playerSubject.GetPlayerTransform;
    }

    public void OnDestroy()
    {
        OnDead = null;
    }
}
