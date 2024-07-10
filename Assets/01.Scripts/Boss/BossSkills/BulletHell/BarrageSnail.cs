using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BarrageSnail : MonoBehaviour, IBossSkill
{
    private IObjectPool<GameObject> pool;

    private float attackDuration = 10f;
    private float attackCooldown = 5f;
    private float attackScale = 0.15f;

    public float Duration => throw new System.NotImplementedException();

    public bool IsActive => throw new System.NotImplementedException();

    public float ElapsedTime => throw new System.NotImplementedException();

    private void Awake()
    {
        
    }

    public void SkillUpdate(float deltaTime)
    {
        throw new System.NotImplementedException();
    }

    public void Activate()
    {
        throw new System.NotImplementedException();
    }

    public void DeActivate()
    {
        throw new System.NotImplementedException();
    }
}
