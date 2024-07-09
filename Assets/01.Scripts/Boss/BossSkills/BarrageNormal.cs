using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BarrageNormal : MonoBehaviour , IBossSkill
{
    private IObjectPool<GameObject> pool;

    private float attackDuration = 3f;
    private float attackCount = 10;
    private float attackScale = 1f;

    private void Awake()
    {
        Addressables.InstantiateAsync(Defines.normalBullet).Completed += InstantiateNormalBullet;
    }

    public void InstantiateNormalBullet(AsyncOperationHandle<GameObject> op)
    {
        var prefab = op.Result;

        pool = new ObjectPool<GameObject>
            (() =>
            {
                var go = Instantiate(prefab);
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

    public void Update()
    {
        throw new System.NotImplementedException();
    }
}
