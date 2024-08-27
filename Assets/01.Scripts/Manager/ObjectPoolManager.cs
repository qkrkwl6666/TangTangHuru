using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ObjectPoolManager : MonoBehaviour
{
    public static IObjectPool<GameObject> expPool;
    public static IObjectPool<GameObject> boomPool;

    public List<GameObject> exps;
    public List<GameObject> booms;

    private void OnDestroy()
    {
        expPool = null;
        boomPool = null;
    }

    private void Awake()
    {
        Addressables.LoadAssetAsync<GameObject>(Defines.exp).Completed += InstantiateExp;
        Addressables.LoadAssetAsync<GameObject>(Defines.monsterBoom).Completed += InstantiateBoomMonster;
    }

    public void InstantiateExp(AsyncOperationHandle<GameObject> op)
    {
        GameObject expPrefab = op.Result;

        expPool = new ObjectPool<GameObject>
            (() =>
            {
                var go = Instantiate(expPrefab);
                go.GetComponent<MonsterExp>().pool = expPool;
                return go;
            },
            (x) =>
            {
                exps.Add(x);
                x.SetActive(true);
            },
            (x) =>
            {
                exps.Remove(x);
                x.SetActive(false);
            },
            (x) => Destroy(x.gameObject),
            true, 10, 200);
    }

    public void InstantiateBoomMonster(AsyncOperationHandle<GameObject> op)
    {
        GameObject expPrefab = op.Result;

        boomPool = new ObjectPool<GameObject>
            (() =>
            {
                var go = Instantiate(expPrefab);
                go.GetComponent<MonsterBoom>().SetObjectPool(boomPool);
                return go;
            },
            (x) =>
            {
                booms.Add(x);
                x.SetActive(true);
            },
            (x) =>
            {
                booms.Remove(x);
                x.SetActive(false);
            },
            (x) => Destroy(x.gameObject),
            true, 10, 200);
    }
}
