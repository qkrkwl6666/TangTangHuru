using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IMonsterState
{

    private MonsterView monsterView;
    private MonsterController monsterController;

    private Spine.TrackEntry prevTrackEntry = null;

    public AttackState(MonsterController monsterController)
    {
        this.monsterController = monsterController;
        this.monsterView = monsterController.MonsterView;
    }

    public void Enter()
    {
        if(prevTrackEntry != null)
        {
            prevTrackEntry.Complete -= OnAttackEndToWalk;
        }

        monsterView.PlayAnimation(Defines.attack, false).Complete += OnAttackEndToWalk;

    }

    public void Exit()
    {
        
    }

    public void Update(float deltaTime)
    {
        
    }

    public void OnAttackEndToWalk(TrackEntry trackEntry)
    {
        prevTrackEntry = trackEntry;
        // 플레이어 공격 처리
        monsterController.PlayerTransform.GetComponent<IDamagable>().OnDamage(monsterController.Monster.Damage);
        monsterController.MonsterStateMachine.TransitionTo(monsterController.MonsterStateMachine.walkState);
    }
}
