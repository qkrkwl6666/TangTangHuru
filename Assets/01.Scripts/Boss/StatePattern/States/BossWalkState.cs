using UnityEngine;
using Transform = UnityEngine.Transform;

public class BossWalkState : BossState
{
    private float coolDown;
    private float time = 0f;
    private Transform playerTransform;

    public BossWalkState(Boss boss, BossView bossView) : base(boss, bossView)
    {
        coolDown = boss.Cooldown;
        playerTransform = boss.PlayerTransform;
    }

    public override void Enter()
    {
        if (boss.isGuardian)
        {
            switch(boss.BossData.Boss_Id)
            {
                case 333001:
                    bossView.PlayAnimation(Defines.idle, true);
                    break;
                case 333002:
                    bossView.PlayAnimation(Defines.groundOut, false).Complete += (x) => 
                    {
                        bossView.PlayAnimation(Defines.walk, true);
                    };
                    break;
                case 333003:

                    break;
            }
        }

        if (boss.Speed <= 0)
        {
            bossView.PlayAnimation(Defines.idle, true);
        }
        else
        {
            bossView.PlayAnimation(Defines.walk, true);
        }
    }

    public override void Exit()
    {
        time = 0f;
    }

    public override void Update(float deltaTime)
    {
        time += deltaTime;

        if (time >= coolDown)
        {
            boss.ChangeState(boss.skillState);
            return;
        }

        if (playerTransform == null) return;

        Vector2 dir = (playerTransform.position - boss.transform.position).normalized;
        boss.transform.Translate(dir * boss.Speed * Time.deltaTime);

    }
}
