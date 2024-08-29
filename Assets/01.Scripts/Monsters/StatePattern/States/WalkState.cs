using UnityEngine;

public enum MonsterMoveType
{
    Chase,
    Direction,
}

public class WalkState : IMonsterState
{
    private UnityEngine.Transform playerTransform;
    private MonsterView monsterView;
    private MonsterController monsterController;
    private Monster monster;

    private MonsterMoveType monsterMoveType;

    private float attackInterval = 1f;
    private float time = 0f;
    private bool isBoom = false;

    public WalkState(MonsterController monsterController, MonsterMoveType monsterMoveType, Monster monster)
    {
        this.monsterMoveType = monsterMoveType;
        this.monsterController = monsterController;
        this.monster = monster;

        monsterView = monsterController.MonsterView;
        playerTransform = monsterController.PlayerTransform;

        attackInterval = monsterController.Monster.AttackInterval;
        isBoom = monster.isBoomType;
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

        if(dis <= monsterController.Monster.Range)
        {
            if (isBoom)
            {
                monster.Die();
            }
        }

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
