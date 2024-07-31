public class MonsterStateMachine
{
    public IMonsterState CurrentState { get; private set; }

    public IdleState idleState;
    public WalkState walkState;
    //public AttackState attackState;

    public MonsterStateMachine(MonsterController monsterController, MonsterMoveType monsterMoveType)
    {
        this.idleState = new IdleState(monsterController);
        this.walkState = new WalkState(monsterController, monsterMoveType);
        //this.attackState = new AttackState(monsterController);
    }

    public void Initialize(IMonsterState monsterState)
    {
        CurrentState = monsterState;
        CurrentState.Enter();
    }

    public void TransitionTo(IMonsterState monsterState)
    {
        CurrentState.Exit();
        CurrentState = monsterState;
        CurrentState.Enter();
    }

    public void Update(float deltaTime)
    {
        if (CurrentState == null) return;

        CurrentState.Update(deltaTime);
    }

}
