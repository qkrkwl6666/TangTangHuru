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

    private float duration = 5f; // 현재 스킬 duration
    private float disableDuration; // 공 duration
    private float time = 0f;
    private int copyCount = 0;
    private bool fission = false;

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

        SkillCount = bossSkillData.Skill_Count;
        DamageFactor = bossSkillData.Damage_Factor;
        SkillRate = bossSkillData.Skill_Rate;
        Damage = damage * DamageFactor;
        enabled = false;
    }

    public void SetReflect(bool fission, float circleScale, float ballSpeed, 
        float disableDuration, int copyCount)
    {
        this.fission = fission;
        this.attackScale = circleScale;
        this.disableDuration = disableDuration;
        this.ballSpeed = ballSpeed;
        this.copyCount = copyCount;
    }

    public void InstantiateBall(AsyncOperationHandle<GameObject> op)
    {
        var prefab = op.Result;

        pool = new ObjectPool<GameObject>
            (() =>
            {
                var go = Instantiate(prefab);
                var reflectBall = go.AddComponent<ReflectBall>();
                reflectBall.SetObjectPool(pool);
                reflectBall.SetDamage(Damage);
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

        if (time > SkillRate) 
        {
            time = 0f;
            Attack();
        }
    }

    public void Attack()
    {
        Vector2 randomDir = Random.insideUnitCircle.normalized;

        var reflectBall = pool.Get().GetComponent<ReflectBall>();

        reflectBall.Init(randomDir, transform, attackScale, 
            ballSpeed, disableDuration, fission, 0f, copyCount);

        reflectBall.transform.position = transform.position;
        currentSkillCount++;

        if (currentSkillCount + 1 > SkillCount)
        {
            IsChange = true;
            return;
        }
    }
}
