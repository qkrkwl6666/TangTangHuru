public class BossDeadState : BossState
{
    private float destoryDuration = 3f;
    private float time = 0f;

    public BossDeadState(Boss boss, BossView bossView) : base(boss, bossView)
    {
    }

    public override void Enter()
    {
        bossView.PlayAnimation(Defines.dead).Complete += (x) =>
        {
            boss.GameUI.ActiveGameClearUI();
        };
    }

    public override void Exit()
    {

    }

    public override void Update(float deltaTime)
    {
        time += deltaTime;

        if (time >= destoryDuration)
        {
            boss.SetTimeScale(0f);
            boss.BossDestory();
        }
    }
}
