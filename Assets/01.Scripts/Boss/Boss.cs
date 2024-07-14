using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : LivingEntity, IPlayerObserver
{
    private List<(IBossSkill skill, float probability)> skills = new List<(IBossSkill, float)>();
    private IBossSkill currentSkill = null;

    private PlayerSubject playerSubject;
    private Transform playerTransform;

    private float totalProbability = 0f;
    private float cooldown = 2f;
    private float time = 0f;

    private float damage = 0f;
    private float speed = 0f;

    public void Initialize(PlayerSubject playerSubject, BossData bossData)
    {
        this.playerSubject = playerSubject;

        startingHealth = bossData.Boss_Hp;
        damage = bossData.Boss_Damage;
        cooldown = bossData.Boss_Cooldown;
        speed = bossData.Boss_MoveSpeed;

        SetBossSkill(bossData);
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

                        bn.SetCountScale(20, 0.2f);
                    }
                    break;
                case 500002:
                    {
                        var bn = AddSkill<BarrageNormal>(skill.Item1, skill.Item2);

                        bn.SetCountScale(40, 0.15f);
                    }
                    break;
                case 500003:
                    {
                        var bn = AddSkill<BarrageSnail>(skill.Item1, skill.Item2);
                    }
                    break;
            }
        }

        SelectSkill();
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
        if (currentSkill.IsChange)
        {
            time += Time.deltaTime;
            
            if (time >= cooldown)
            {
                SelectSkill();
            }
        }
        else
        {
            currentSkill?.SkillUpdate(Time.deltaTime);
        }

        
    }

    public void SelectSkill()
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
    }

    public void IObserverUpdate()
    {
        playerTransform = playerSubject.GetPlayerTransform;
    }
}
