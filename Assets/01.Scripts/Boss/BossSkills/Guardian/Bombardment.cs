using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Bombardment : MonoBehaviour, IBossSkill
{
    private IObjectPool<GameObject> pool;

    public int SkillCount { get; set; } = 1;
    public bool IsChange { get; set; } = false;
    public float SkillRate { get; set; } = 1f;
    public float DamageFactor { get; set; } = 1f;
    public float Damage { get; set; } = 1f;

    private int currentSkillCount = 0;

    private Transform playerTransform;
    private BossView bossView;
    private Boss boss;

    private float time = 0f;

    private void Awake()
    {
        boss = GetComponent<Boss>();
        bossView = GetComponentInChildren<BossView>();
    }

    public void Initialize(BossSkillData bossSkillData, float damage)
    {
        Addressables.LoadAssetAsync<GameObject>(Defines.boom)
            .Completed += InstantiateBoom;

        SkillCount = bossSkillData.Skill_Count;
        DamageFactor = bossSkillData.Damage_Factor;
        SkillRate = bossSkillData.Skill_Rate;
        Damage = damage * DamageFactor;
        enabled = false;

        playerTransform = boss.PlayerTransform;
    }

    public void InstantiateBoom(AsyncOperationHandle<GameObject> op)
    {
        var prefab = op.Result;

        pool = new ObjectPool<GameObject>
            (() =>
            {
                var go = Instantiate(prefab);
                var boom = go.GetComponent<Boom>();
                boom.SetObjectPool(pool);
                boom.Initialize(1.5f, Damage);
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

    public void SkillUpdate(float deltaTime)
    {
        time += deltaTime;

        if(time >= SkillRate)
        {
            time = 0f;
            Attack();
        }
    }

    public void Attack()
    {
        currentSkillCount++;

        //int random = Random.Range(0, 2);

        //(random == 0 ? (System.Action)FireCurrentPosition : FirePredictedPosition)();

        FireCurrentPosition();

        if (currentSkillCount + 1 > SkillCount)
        {
            IsChange = true;
            return;
        }
    }

    public void FireCurrentPosition()
    {
        if (playerTransform == null) return;

        var boom = pool.Get();

        boom.transform.position = playerTransform.position;
    }

    public void FirePredictedPosition()
    {
        if (playerTransform == null) return;


    }
}
