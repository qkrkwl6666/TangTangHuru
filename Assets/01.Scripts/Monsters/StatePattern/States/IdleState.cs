using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IMonsterState
{
    private MonsterView monsterView;
    private MonsterController monsterController;

    public IdleState(MonsterController monsterController)
    {
        this.monsterController = monsterController;
        this.monsterView = monsterController.MonsterView; 
    }
    public void Enter()
    {
        monsterView.PlayAnimation(Defines.idle, true);
    }

    public void Exit()
    {
        
    }

    public void Update(float deltaTime)
    {
        
    }
}
