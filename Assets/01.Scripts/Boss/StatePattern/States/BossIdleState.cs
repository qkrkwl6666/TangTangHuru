using UnityEngine;

public class BossIdleState : BossState
{
    private float coolDown;
    private float time = 0f;
    private Transform playerTransform;


    public BossIdleState(Boss boss, BossView bossView) : base(boss, bossView)
    {
        coolDown = boss.Cooldown;
        playerTransform = boss.PlayerTransform;
    }

    public override void Enter()
    {
        bossView.PlayAnimation(Defines.idle, true);
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
        }
    }
}
