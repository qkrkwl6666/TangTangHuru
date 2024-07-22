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

    // 보스 상태 패턴
    private BossState currentState;

    public BossWalkState walkState;
    public BossSkillState skillState;

    // View
    private MonsterView monsterView;

    private InGameUI gameUI;

    private void Awake()
    {
        monsterView = GetComponentInChildren<MonsterView>();
        gameUI = GameObject.FindWithTag("InGameUI").GetComponent<InGameUI>();
    }

    public void Initialize(PlayerSubject playerSubject, BossData bossData)
    {
        this.playerSubject = playerSubject;

        playerSubject.AddObserver(this);

        startingHealth = bossData.Boss_Hp;
        damage = bossData.Boss_Damage;
        Cooldown = bossData.Boss_Cooldown;
        Speed = bossData.Boss_MoveSpeed;

        walkState = new BossWalkState(this, monsterView);
        skillState = new BossSkillState(this, monsterView);

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
        currentState.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Vector2 dir = (PlayerTransform.position - gameObject.transform.position).normalized;

        monsterView.skeletonAnimation.skeleton.ScaleX = dir.x < 0 ? -1f : 1f;
    }

    public IBossSkill SelectSkill()
    {
        if (currentSkill != null)
        {
            currentSkill.DeActivate();
        }

        time = 0f;
        currentSkill = null;

        float random = Random.Range(0, totalProbability);

        float currentProbability = 0f;

        foreach (var (skill, probability) in skills)
        {
            currentProbability += probability;

            if(random <= currentProbability)
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

    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);

        gameUI.UpdateBossHpBar(health / startingHealth);
    }

    public void IObserverUpdate()
    {
        PlayerTransform = playerSubject.GetPlayerTransform;
    }
}
