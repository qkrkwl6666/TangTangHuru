public class BossSkillState : BossState
{

    public float endTime = 1.2f;  // 1.267초에서 idle로 전환되기 직전


    public BossSkillState(Boss boss, BossView bossView) : base(boss, bossView)
    {

    }

    public override void Enter()
    {
        currentBossSkill = boss.SelectSkill();

        if (!boss.isGuardian)
        {
            bossView.PlayAnimation(Defines.idle, true);
            return;
        }

        switch (boss.BossData.Boss_Id)
        {
            case 333001: // 돌진 가디언
                bossView.PlayAnimation(Defines.attack1, true);
                break;
            case 333002: // 포격 가디언
                bossView.PlayAnimation(Defines.groundIn, false).Complete += (x) =>
                {
                    bossView.PlayAnimation(Defines.idle, true);
                };
                break;
            case 333003: // 

                break;
        }


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
