using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BarrageSnail : MonoBehaviour, IBossSkill
{
    private IObjectPool<GameObject> pool;

    private float attackScale = 0.15f;

    // 공격 주기 시간
    private float attackTime = 0f;
    private float attackDuration = 0.1f;

    private int currentSkillCount = 0;

    public float Damage {  get; set; }
    public int SkillCount { get; set; } = 3;
    public bool IsChange { get; set; } = false;
    public float SkillRate { get; set; } = 5f;
    public float DamageFactor { get; set; } = 0f;

    private int currentIndex = 0;
    private int maxIndex = 20;

    private void Awake()
    {
        Addressables.LoadAssetAsync<GameObject>(Defines.snailBullet).Completed += InstantiateSnailBullet;
        enabled = false;
    }

    public void InstantiateSnailBullet(AsyncOperationHandle<GameObject> op)
    {
        var prefab = op.Result;

        pool = new ObjectPool<GameObject>
            (() =>
            {
                var go = Instantiate(prefab);
                var barrage = go.AddComponent<Barrage>();
                barrage.SetObjectPool(pool);
                return go;
            },
            (x) =>
            {
                x.SetActive(true);
            },
            (x) =>
            {
                x.SetActive(false);
            },
            (x) => Destroy(x.gameObject),
            true, 10, 100);
    }

    public void SkillUpdate(float deltaTime)
    {
        attackTime += deltaTime;

        if(attackTime >= attackDuration)
        {
            attackTime = 0f;
            Attack();
        }
    }

    public void Attack()
    {
        float angle = ((360 / maxIndex) * currentIndex) * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        if (pool == null) return;
        var go = pool.Get();

        go.GetComponent<Barrage>().Init(dir, transform, attackScale);

        currentIndex++;

        if (currentIndex >= maxIndex)
        {
            currentSkillCount++;
            currentIndex = 0;
        }

        if (currentSkillCount >= SkillCount) 
        {
            IsChange = true;
            return;
        }
            
    }

    public void Activate()
    {
        enabled = true;
    }

    public void DeActivate()
    {
        IsChange = false;
        enabled = false;
        attackTime = 0f;
        currentSkillCount = 0;
    }

    public void Initialize(BossSkillData bossSkillData, float damage)
    {
        //throw new NotImplementedException();
    }
}
