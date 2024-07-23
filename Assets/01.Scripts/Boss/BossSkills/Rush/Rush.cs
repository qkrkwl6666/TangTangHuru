using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rush : MonoBehaviour, IBossSkill
{
    public float Damage {  get; set; }
    public int SkillCount { get; set; }
    public bool IsChange { get; set; } = false;
    public float SkillRate { get; set; } = 5f;
    public float DamageFactor { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Activate()
    {
        throw new System.NotImplementedException();
    }

    public void DeActivate()
    {
        throw new System.NotImplementedException();
    }

    public void Initialize(BossSkillData bossSkillData, float damage)
    {
        throw new System.NotImplementedException();
    }

    public void SkillUpdate(float deltaTime)
    {
        throw new System.NotImplementedException();
    }
}
