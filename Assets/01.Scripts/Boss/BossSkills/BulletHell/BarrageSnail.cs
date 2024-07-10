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

    // 카운트 재는 시간
    private float time = 0f;
    private float duration = 20f;

    // 공격 주기 시간
    private float attackTime = 0f;
    private float attackDuration = 0.05f;

    private int currentSkillCount = 0;
    public int SkillCount { get; set; } = 3;
    public bool IsChange { get; set; } = false;
    public float Cooldown { get; set; } = 5f;

    private int currentIndex = 0;
    private int maxIndex = 20;

    private void Awake()
    {
        Addressables.LoadAssetAsync<GameObject>(Defines.snailBullet).Completed += InstantiateSnailBullet;
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
        time += deltaTime;
        attackTime += deltaTime;

        if (time >= duration)
        {
            time = 0f;
            SkillCount++;
            if (currentSkillCount == SkillCount) 
            {
                IsChange = true;
                return;
            }
        }

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

        var go = pool.Get();

        go.GetComponent<Barrage>().Init(dir, transform, attackScale);

        currentIndex++;

        if (currentIndex >= maxIndex) currentIndex = 0;
    }

    public void Activate()
    {
        enabled = true;
    }

    public void DeActivate()
    {
        IsChange = false;
        enabled = false;
        time = 0f;
        attackTime = 0f;
        currentSkillCount = 0;
    }
}
