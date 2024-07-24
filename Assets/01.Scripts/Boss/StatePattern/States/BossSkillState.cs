using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillState : BossState
{
    public BossSkillState(Boss boss, MonsterView monsterView) : base(boss, monsterView)
    {

    }

    public override void Enter()
    {
        currentBossSkill = boss.SelectSkill();

        monsterView.PlayAnimation(Defines.idle, true);
    }

    public override void Exit()
    {
        currentBossSkill = null;
    }

    public override void Update(float deltaTime)
    {
        if (currentBossSkill.IsChange)
        {
            boss.ChangeState(boss.walkState);
            return;
        }

        currentBossSkill.SkillUpdate(deltaTime);
    }
}
