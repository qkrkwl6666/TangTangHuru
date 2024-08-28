using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

public class RangeArea : MonoBehaviour, IBossSkill
{
    private IObjectPool<GameObject> pool;

    public float Damage { get; private set; }

    public int SkillCount { get; set; }
    public bool IsChange { get; set; }
    public float SkillRate { get; set; }
    public float DamageFactor { get; set; }

    private int currentSkillCount = 0;

    public float Distance { get; private set; } = 20f;

    private Transform playerTransform;
    private float time = 0f;

    private float boomDuration = 3f;
    private float scale = 3f;

    public void Activate()
    {
        enabled = true;
    }

    public void DeActivate()
    {
        time = 0f;
        currentSkillCount = 0;
        IsChange = false;
        enabled = false;
    }

    public void SetScaleDuration(float duration, float scale)
    {
        boomDuration = duration;
        this.scale = scale;
    }

    public void Initialize(BossSkillData bossSkillData, float damage)
    {
        Addressables.LoadAssetAsync<GameObject>(bossSkillData.Preafab_Id)
            .Completed += InstantiateArea;

        SkillCount = bossSkillData.Skill_Count;
        DamageFactor = bossSkillData.Damage_Factor;
        SkillRate = bossSkillData.Skill_Rate;
        Damage = damage * DamageFactor;
        enabled = false;

        playerTransform = GetComponent<Boss>().PlayerTransform;
    }

    public void InstantiateArea(AsyncOperationHandle<GameObject> op)
    {
        var prefab = op.Result;

        pool = new ObjectPool<GameObject>
            (() =>
            {
                var go = Instantiate(prefab);
                var area = go.AddComponent<Area>();
                area.SetObjectPool(pool);
                area.SetDamage(Damage);
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

        if (time >= SkillRate)
        {
            Attack();
        }
    }

    public void Attack()
    {
        time = 0f;
        currentSkillCount++;
        Vector2 randomPos = (Vector2)transform.position + Random.insideUnitCircle * Random.Range(5f, Distance);

        var area = pool.Get();
        area.transform.position = randomPos;
        area.GetComponent<Area>().InitSkill(scale, boomDuration, playerTransform);

        if (currentSkillCount >= SkillCount)
        {
            IsChange = true;
        }
    }
}
