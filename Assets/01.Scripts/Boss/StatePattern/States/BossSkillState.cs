public class BossSkillState : BossState
{

    public float endTime = 1.2f;  // 1.267�ʿ��� idle�� ��ȯ�Ǳ� ����


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
            case 333001: // ���� �����
                bossView.PlayAnimation(Defines.attack1, true);
                break;
            case 333002: // ���� �����
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
