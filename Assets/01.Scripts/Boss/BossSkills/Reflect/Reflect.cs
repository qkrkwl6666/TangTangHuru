using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Reflect : MonoBehaviour, IBossSkill
{
    private IObjectPool<GameObject> pool;
    public int SkillCount { get ; set ; }
    public bool IsChange { get ; set ; }
    public float SkillRate { get ; set ; }
    public float DamageFactor { get ; set ; }
    public float Damage { get ; set ; }

    private float ballSpeed = 5f;
    public int currentSkillCount = 0;
    private float attackScale = 1.0f;

    private float duration = 5f;
    private float time = 0f;

    public void Activate()
    {
        enabled = true;
    }

    public void DeActivate()
    {
        time = 0f;
        enabled = false;
        currentSkillCount = 0;
        IsChange = false;
    }

    public void Initialize(BossSkillData bossSkillData, float damage)
    {
        Addressables.LoadAssetAsync<GameObject>(bossSkillData.Preafab_Id)
             .Completed += InstantiateBall;
    }

    public void InstantiateBall(AsyncOperationHandle<GameObject> op)
    {
        var prefab = op.Result;

        pool = new ObjectPool<GameObject>
            (() =>
            {
                var go = Instantiate(prefab);
                //var barrage = go.AddComponent<Barrage>();
                //barrage.SetObjectPool(pool);
                //barrage.SetDamage(Damage);
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

        if (time > attackScale) 
        {

        }
    }

    public void Attack()
    {
        Vector2 randomDir = Random.insideUnitCircle;
    }
}
