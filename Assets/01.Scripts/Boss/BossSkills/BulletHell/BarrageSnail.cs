using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BarrageSnail : MonoBehaviour, IBossSkill
{
    private IObjectPool<GameObject> pool;

    private float attackScale = 0.8f;

    // 공격 주기 시간
    private float attackTime = 0f;
    //private float attackDuration = 0.05f;

    private int currentSkillCount = 0;

    private float ballSpeed = 7f;

    public float Damage { get; set; }
    public int SkillCount { get; set; } = 3;
    public bool IsChange { get; set; } = false;
    public float SkillRate { get; set; } = 5f;
    public float DamageFactor { get; set; } = 0f;

    private int currentIndex = 0;
    private int maxIndex = 30;

    public void Initialize(BossSkillData bossSkillData, float damage)
    {
        Addressables.LoadAssetAsync<GameObject>(bossSkillData.Preafab_Id)
            .Completed += InstantiateSnailBullet;

        SkillCount = bossSkillData.Skill_Count;
        DamageFactor = bossSkillData.Damage_Factor;
        SkillRate = bossSkillData.Skill_Rate;
        Damage = damage * DamageFactor;
        enabled = false;
    }

    public void Init(float skillRate, float attackScale, float ballSpeed)
    {
        SkillRate = skillRate;
        this.ballSpeed = ballSpeed;
        this.attackScale = attackScale;
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
        attackTime += deltaTime;

        if (attackTime >= SkillRate)
        {
            attackTime = 0f;
            Attack();
        }
    }

    public void Attack()
    {

        float angle = ((360 / maxIndex) * currentIndex) * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        if (pool == null) return;
        var go = pool.Get();

        go.GetComponent<Barrage>().Init(dir, transform, attackScale, ballSpeed);

        currentIndex++;

        if (currentIndex >= maxIndex)
        {
            currentSkillCount++;
            currentIndex = 0;
        }

        if (currentSkillCount >= SkillCount)
        {
            IsChange = true;
            return;
        }

    }

    public void Activate()
    {
        enabled = true;
    }

    public void DeActivate()
    {
        IsChange = false;
        enabled = false;
        attackTime = 0f;
        currentSkillCount = 0;
    }
}
