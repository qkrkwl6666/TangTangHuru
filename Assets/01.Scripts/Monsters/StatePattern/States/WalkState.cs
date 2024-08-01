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

    private float attackInterval = 1f;
    private float time = 0f;

    public WalkState(MonsterController monsterController, MonsterMoveType monsterMoveType)
    {
        this.monsterMoveType = monsterMoveType;
        this.monsterController = monsterController;

        monsterView = monsterController.MonsterView;
        playerTransform = monsterController.PlayerTransform;

        attackInterval = monsterController.Monster.AttackInterval;
    }

    public void Enter()
    {
        //monsterView.PlayAnimation(Defines.walk, true);
    }

    public void Exit()
    {

    }

    public void Update(float deltaTime)
    {
        float dis = Vector2.Distance(playerTransform.position, monsterView.transform.position);
        
        time += deltaTime;
        
        if (dis <= monsterController.Monster.Range && time >= attackInterval)
        {
            // monsterController.MonsterStateMachine.TransitionTo
            //     (monsterController.MonsterStateMachine.attackState);
        
            monsterController.PlayerTransform.GetComponent<IDamagable>().OnDamage(monsterController.Monster.Damage, 0);
            time = 0f;
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
