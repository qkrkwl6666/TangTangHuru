using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

public class RangeSkill : MonoBehaviour
{
    private IObjectPool<GameObject> pool;

    private float damage;
    private Transform playerTransform;
    private float coolDown = 5f;
    private float time = 0f;
    private float speed = 5f;

    public void Update()
    {
        time += Time.deltaTime;

        if (time >= coolDown) 
        {
            time = 0f;
            Attack();
        }
    }

    public void Initialize(MonsterData monsterData, Transform transform)
    {
        Addressables.LoadAssetAsync<GameObject>(monsterData.Skill_Prefab)
                .Completed += InstantiateTornado;

        damage = monsterData.Monster_Damage;
        coolDown = monsterData.AttackInterval;
        playerTransform = transform;
    }
    public void SetObjectPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }

    public void InstantiateTornado(AsyncOperationHandle<GameObject> op)
    {
        var prefab = op.Result;

        pool = new ObjectPool<GameObject>
            (() =>
            {
                var go = Instantiate(prefab);
                var tornado = go.AddComponent<Tornado>();
                tornado.SetObjectPool(pool);
                tornado.SetDamage(damage);
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

    public void Attack()
    {
        var skill = pool.Get().GetComponent<Tornado>();

        skill.transform.position = transform.position;
        Vector2 dir = (playerTransform.position - transform.position).normalized;
        skill.Init(dir, 10f, speed, 0.3f);
    }
}
