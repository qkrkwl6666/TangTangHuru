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

    public bool isGuardian = false;

    // 골드
    public int Gold { get; private set; }

    // 보스 상태 패턴
    private BossState currentState;

    public BossIdleState idleState;
    public BossWalkState walkState;
    public BossSkillState skillState;
    public BossDeadState deadState;

    // View
    private BossView bossView;
    public BossData BossData { get; private set; }
    public InGameUI GameUI { get; private set; }

    private void Awake()
    {
        bossView = GetComponentInChildren<BossView>();
        GameUI = GameObject.FindWithTag("InGameUI").GetComponent<InGameUI>();
    }

    public void Initialize(PlayerSubject playerSubject, BossData bossData)
    {
        BossData = bossData;
        this.playerSubject = playerSubject;

        playerSubject.AddObserver(this);

        startingHealth = bossData.Boss_Hp;
        damage = bossData.Boss_Damage;
        Cooldown = bossData.Boss_Cooldown;
        Speed = bossData.Boss_MoveSpeed;
        Gold = bossData.Gold;

        //idleState = new BossIdleState(this, bossView);
        walkState = new BossWalkState(this, bossView);
        skillState = new BossSkillState(this, bossView);
        deadState = new BossDeadState(this, bossView);

        SetBossSkill(bossData);

        AwakeHealth();

        SetScale();
    }
    public void SetBossSkill(BossData bossData)
    {
        var skillList = bossData.GetBossSkillId();

        foreach (var skill in skillList)
        {
            if (skill.Item1 == -1) break;

            var skillData = DataTableManager.Instance.Get<BossSkillTable>(DataTableManager.bossSkill)
                .Get(skill.Item1.ToString());

            switch(skillData.Skill_Type)
            {
                case 1:
                    {
                        var bn = AddSkill<BarrageNormal>(skill.Item1, skill.Item2);
                        bn.SetCountScale(skillData.Obect_Count, skillData.Object_Scale,
                            skillData.Random_Count, skillData.Obejct_Speed);
                    }
                    break;
                case 2:
                    {
                        var bs = AddSkill<BarrageSnail>(skill.Item1, skill.Item2);
                        bs.Init(skillData.Skill_Rate, skillData.Object_Scale, skillData.Obejct_Speed);
                    }
                    break;
                case 3:
                    {
                        var rush = AddSkill<Rush>(skill.Item1, skill.Item2);
                    }
                    break;
                case 4:
                    {
                        var ra = AddSkill<RangeArea>(skill.Item1, skill.Item2);
                    }
                    break;
                case 5:
                    {
                        var laser = AddSkill<LaserSkill>(skill.Item1, skill.Item2);
                        laser.SetLaser(skillData.Obect_Count, skillData.Skill_Count, skillData.Object_Scale, skillData.Obejct_Speed, 2f);
                        laser.laserSetting.rotationTypes.Add(LaserSkill.RotationType.Right);
                    }
                    break;
                case 6:
                    {
                        var laser = AddSkill<LaserSkill>(skill.Item1, skill.Item2);
                        laser.SetLaser(skillData.Obect_Count, skillData.Skill_Count, skillData.Object_Scale, skillData.Obejct_Speed, 2f);
                        laser.laserSetting.rotationTypes.Add(LaserSkill.RotationType.Right);
                        laser.laserSetting.rotationTypes.Add(LaserSkill.RotationType.Left);
                    }
                    break;
                case 7:
                    // 분열 X
                    {
                        var reflect = AddSkill<Reflect>(skill.Item1, skill.Item2);
                        reflect.SetReflect(false, skillData.Object_Scale, skillData.Obejct_Speed, 
                            skillData.Disable_Duration, skillData.Obect_Count, skillData.Random_Count);
                    }
                    break;
                case 8:
                    // 분열 O
                    {
                        var reflect = AddSkill<Reflect>(skill.Item1, skill.Item2);
                        reflect.SetReflect(true, skillData.Object_Scale, skillData.Obejct_Speed,
                            skillData.Disable_Duration, skillData.Obect_Count, skillData.Random_Count);
                    }
                    break;
                case 9:
                    {
                        var sword = AddSkill<SwordSkill>(skill.Item1, skill.Item2);
                        sword.SetSwordSkill(skillData.Obect_Count, PlayerTransform, bossView);
                    }
                    break;
                case 10:
                    {
                        var sword = AddSkill<SwordSkill>(skill.Item1, skill.Item2);
                        sword.SetSwordSkill(skillData.Obect_Count, PlayerTransform, bossView, true);
                    }
                    break;
                case 11:
                    {
                        var dash = AddSkill<Dash>(skill.Item1, skill.Item2);
                    }
                    break;
                case 12:
                    {
                        var bombardment = AddSkill<Bombardment>(skill.Item1, skill.Item2);
                    }
                    break;
                case 13:
                    {
                        var stunned = AddSkill<Stunned>(skill.Item1, skill.Item2);
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

        bossView.skeletonAnimation.skeleton.ScaleX = dir.x < 0 ? -1f : 1f;
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

        if (isGuardian)
        {
            gameObject.SetActive(false);
            return;
        }
            

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

    public void SetScale()
    {
        gameObject.transform.localScale = new Vector3(BossData.SizeScale, BossData.SizeScale);
    }
}
