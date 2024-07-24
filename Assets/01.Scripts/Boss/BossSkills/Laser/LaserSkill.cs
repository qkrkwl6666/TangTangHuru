using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;



public class LaserSkill : MonoBehaviour, IBossSkill
{
    public enum RotationType
    {
        Right,
        Left,
    }
    private IObjectPool<GameObject> pool;

    int IBossSkill.SkillCount { get ; set ; }
    bool IBossSkill.IsChange { get ; set ; }
    float IBossSkill.SkillRate { get ; set ; }
    float IBossSkill.DamageFactor { get ; set ; }

    float rotationTime = 0f;

    public List<RotationType> rotationTypes = new List<RotationType>();

    void IBossSkill.Activate()
    {
        
    }

    void IBossSkill.DeActivate()
    {
        
    }

    void IBossSkill.Initialize(BossSkillData bossSkillData, float damage)
    {
        
    }

    void IBossSkill.SkillUpdate(float deltaTime)
    {
        
    }
}
