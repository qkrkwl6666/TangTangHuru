public class BossSkillState : BossState
{
    public BossSkillState(Boss boss, BossView bossView) : base(boss, bossView)
    {

    }

    public override void Enter()
    {
        currentBossSkill = boss.SelectSkill();

        bossView.PlayAnimation(Defines.idle, true);
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
