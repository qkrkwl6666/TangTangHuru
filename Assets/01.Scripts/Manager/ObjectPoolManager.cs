using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ObjectPoolManager : MonoBehaviour
{
    public static IObjectPool<GameObject> expPool;
    public List<GameObject> exps;

    private void OnDestroy()
    {
        expPool = null;
    }

    private void Awake()
    {
        Addressables.LoadAssetAsync<GameObject>(Defines.exp).Completed += ExpInstantiate;
    }

    public void ExpInstantiate(AsyncOperationHandle<GameObject> op)
    {
        GameObject expPrefab = op.Result;

        expPool = new ObjectPool<GameObject>
            ( () => 
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
            true,
            10, 200);
    }
}
