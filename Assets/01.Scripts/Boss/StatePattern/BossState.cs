public abstract class BossState
{
    protected Boss boss;
    protected IBossSkill currentBossSkill;
    protected MonsterView monsterView;

    public BossState(Boss boss, MonsterView monsterView)
    {
        this.boss = boss;
        this.monsterView = monsterView;
    }

    public abstract void Enter();
    public abstract void Update(float deltaTime);
    public abstract void Exit();
}
