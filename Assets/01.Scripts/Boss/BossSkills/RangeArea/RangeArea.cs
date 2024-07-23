using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class RangeArea : MonoBehaviour, IBossSkill
{
    public int SkillCount { get ; set ; }
    public bool IsChange { get ; set ; }
    public float SkillRate { get ; set ; }
    public float DamageFactor { get ; set ; }

    private Transform playerTransform;
    private float time = 0f;

    private float boomDuration = 0f;
    private float scale = 0f;

    public void Activate()
    {
        
    }

    public void DeActivate()
    {
        
    }

    public void SetScaleDuration(float duration, float scale)
    {
        boomDuration = duration;
        this.scale = scale;
    }

    public void Initialize(BossSkillData bossSkillData, float damage)
    {

    }

    public void SkillUpdate(float deltaTime)
    {
        time += deltaTime;

        if (time >= SkillRate) 
        {
            Attack();
        }
    }

    public void Attack()
    {
        //Addressables.InstantiateAsync(Defines.rangeArea);
    }
}
