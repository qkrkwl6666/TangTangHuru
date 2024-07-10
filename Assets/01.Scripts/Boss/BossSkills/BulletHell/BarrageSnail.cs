using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BarrageSnail : MonoBehaviour, IBossSkill
{
    private IObjectPool<GameObject> pool;

    private float attackScale = 0.15f;

    public int SkillCount { get; set; } = 5;
    public bool IsChange { get; set; } = false;
    public float Cooldown { get; set; } = 5f;

    private void Awake()
    {
        
    }

    public void SkillUpdate(float deltaTime)
    {
        throw new System.NotImplementedException();
    }

    public void Activate()
    {
        enabled = true;
    }

    public void DeActivate()
    {
        enabled = false;
    }
}
