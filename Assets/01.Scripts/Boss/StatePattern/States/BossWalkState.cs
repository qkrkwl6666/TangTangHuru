using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Transform = UnityEngine.Transform;

public class BossWalkState : BossState
{
    private float coolDown;
    private float time = 0f;
    private Transform playerTransform; 


    public BossWalkState(Boss boss, MonsterView monsterView) : base(boss, monsterView)
    {
        coolDown = boss.Cooldown;
        playerTransform = boss.PlayerTransform;
    }

    public override void Enter()
    {
        monsterView.PlayAnimation(Defines.walk, true);
    }

    public override void Exit()
    {
        time = 0f;
    }

    public override void Update(float deltaTime)
    {
        time += deltaTime;

        if (time >= coolDown) 
        {
            boss.ChangeState(boss.skillState);
            return;
        }

        if (playerTransform == null) return;

        Vector2 dir = (playerTransform.position - boss.transform.position).normalized;
        boss.transform.Translate(dir * boss.Speed * Time.deltaTime);

    }
}
