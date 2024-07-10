using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BarrageNormal : MonoBehaviour, IBossSkill
{
    private IObjectPool<GameObject> pool;

    private float attackCooldown = 3f;
    private float attackCount = 20;
    private float attackScale = 0.1f;
    private float time = 0f;

    public float Duration => throw new System.NotImplementedException();

    public bool IsActive => throw new System.NotImplementedException();

    public float ElapsedTime => throw new System.NotImplementedException();

    private void Awake()
    {
        Addressables.LoadAssetAsync<GameObject>(Defines.normalBullet).Completed += InstantiateNormalBullet;
    }
    public void InstantiateNormalBullet(AsyncOperationHandle<GameObject> op)
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

        if (time >= attackCooldown)
        {
            time = 0f;
            Attack();   
        }
    }

    public void Attack()
    { 
        for (int i = 0; i < attackCount; i++) 
        {
            CirlcePosition(i);
        }
    }

    public void CirlcePosition(int index)
    {
        float angle = ((360 / attackCount) * index) * Mathf.Deg2Rad;

        Vector2 CirclePos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        var go = pool.Get();
        go.GetComponent<Barrage>().Init(CirclePos, transform, attackScale);
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
