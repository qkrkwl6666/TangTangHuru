using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeadState : BossState
{
    private float destoryDuration = 3f;
    private float time = 0f;

    public BossDeadState(Boss boss, MonsterView monsterView) : base(boss, monsterView)
    {
    }

    public override void Enter()
    {
        monsterView.PlayAnimation(Defines.dead).Complete += (x) =>
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

        if(time >= destoryDuration)
        {
            boss.SetTimeScale(0f);
            boss.BossDestory();
        }
    }
}
