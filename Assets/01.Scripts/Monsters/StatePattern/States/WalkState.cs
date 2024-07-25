using UnityEngine;

public enum MonsterMoveType
{
    Chase,
    Direction,
}

public class WalkState : IMonsterState
{
    private Transform playerTransform;
    private MonsterView monsterView;
    private MonsterController monsterController;

    private MonsterMoveType monsterMoveType;

    public WalkState(MonsterController monsterController, MonsterMoveType monsterMoveType)
    {
        this.monsterMoveType = monsterMoveType;
        this.monsterController = monsterController;

        monsterView = monsterController.MonsterView;
        playerTransform = monsterController.PlayerTransform;
    }

    public void Enter()
    {
        monsterView.PlayAnimation(Defines.walk, true);
    }

    public void Exit()
    {

    }

    public void Update(float deltaTime)
    {
        float dis = Vector2.Distance(playerTransform.position, monsterView.transform.position);

        if (dis <= monsterController.Monster.Range)
        {
            monsterController.MonsterStateMachine.TransitionTo
                (monsterController.MonsterStateMachine.attackState);

            return;
        }

        switch (monsterMoveType)
        {
            case MonsterMoveType.Chase:
                monsterController.ChasePlayer(deltaTime);
                break;
            case MonsterMoveType.Direction:
                monsterController.MoveToInitialPlayerPosition(deltaTime);
                break;
        }
    }
}
