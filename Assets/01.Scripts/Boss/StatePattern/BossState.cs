public abstract class BossState
{
    protected Boss boss;
    protected IBossSkill currentBossSkill;
    protected BossView bossView;

    public BossState(Boss boss, BossView bossView)
    {
        this.boss = boss;
        this.bossView = bossView;
    }

    public abstract void Enter();
    public abstract void Update(float deltaTime);
    public abstract void Exit();
}
