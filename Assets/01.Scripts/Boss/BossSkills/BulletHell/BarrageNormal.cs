using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BarrageNormal : MonoBehaviour, IBossSkill
{
    private IObjectPool<GameObject> pool;

    private int attackCount = 20; // 총알 개수
    private float attackScale = 0.2f;
    private float time = 0f;
    private int randomAttackCount = 10;
    private float ballSpeed = 5f;

    public int currentSkillCount = 0;

    public float Damage { get; set; }
    public int SkillCount { get; set; } = 5;
    public bool IsChange { get; set; } = false;
    public float SkillRate { get; set; } = 5f;
    public float DamageFactor { get; set; } = 1f;

    private void Awake()
    {
        
    }

    public void SetCountScale(int count, float scale)
    {
        attackCount = count;
        attackScale = scale;
    }

    public void Initialize(BossSkillData bossSkillData, float damage)
    {
        Addressables.LoadAssetAsync<GameObject>(bossSkillData.Preafab_Id)
            .Completed += InstantiateNormalBullet;

        SkillCount = bossSkillData.Skill_Count;
        DamageFactor = bossSkillData.Damage_Factor;
        SkillRate = bossSkillData.Skill_Rate;
        Damage = damage * DamageFactor;
        enabled = false;
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
                barrage.SetDamage(Damage);
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
            time = 0f;
            Attack();   
        }
    }

    public void Attack()
    {
        currentSkillCount++;

        float randomCount = attackCount + Random.Range(0, randomAttackCount);

        for (int i = 0; i < randomCount; i++) 
        {
            CirlcePosition(i, randomCount);
        }

        if (currentSkillCount + 1 > SkillCount)
        {
            IsChange = true;
            return;
        }
    }

    // Todo : 이름 변경 하기
    public void CirlcePosition(int index, float randomCount)
    {
        float angle = ((360 / randomCount) * index) * Mathf.Deg2Rad;

        Vector2 CirclePos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        if (pool == null) return;
        var go = pool.Get();
        go.GetComponent<Barrage>().Init(CirclePos, transform, attackScale, ballSpeed);
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
}
