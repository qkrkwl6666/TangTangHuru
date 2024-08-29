using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

public class SwordSkill : MonoBehaviour, IBossSkill
{
    private IObjectPool<GameObject> pool;
    public int SkillCount { get ; set ; }
    public bool IsChange { get ; set ; }
    public float SkillRate { get ; set ; }
    public float DamageFactor { get ; set ; }
    public float Damage { get ; set ; }
    public float time = 0f;
    public float currentSkillCount = 0f;
    private int attackCount = 1;

    private float disableDuration = 10f;
    private float scale = 1f;
    private float speed = 10f;
    private float maxAngle = 180;

    private Transform playerTransform;
    private BossView bossView;

    private bool allAttackCount = false;
    private int randomAttackCount = 0;

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

    public void SetSwordSkill(int attackCount, Transform transform, BossView bossView, bool isAllAttck = false)
    {
        this.attackCount = attackCount;
        playerTransform = transform;
        this.bossView = bossView;
        this.allAttackCount = isAllAttck;
    }

    public void Initialize(BossSkillData bossSkillData, float damage)
    {
        Addressables.LoadAssetAsync<GameObject>(bossSkillData.Preafab_Id)
            .Completed += InstantiateTornado;

        SkillCount = bossSkillData.Skill_Count;
        DamageFactor = bossSkillData.Damage_Factor;
        SkillRate = bossSkillData.Skill_Rate;
        Damage = damage * DamageFactor;
        scale = bossSkillData.Object_Scale;
        randomAttackCount = bossSkillData.Random_Count;
        speed = bossSkillData.Obejct_Speed;
        disableDuration = bossSkillData.Disable_Duration;
        enabled = false;
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
                tornado.SetDamage(Damage);
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

        bossView.PlayAnimation(Defines.attack).Complete += (x) => 
        {
            if(allAttackCount)
            {
                for(int i =0; i < attackCount; i++)
                {
                    CirlcePosition(i, attackCount);
                }
            }
            else
            {
                for (int i = 0; i < attackCount; i++)
                {
                    if (attackCount == 1)
                    {
                        Vector2 playerDir = (playerTransform.position - transform.position).normalized;
                        var tornado2 = pool.Get().GetComponent<Tornado>().GetComponent<Tornado>();

                        tornado2.transform.position = transform.position;
                        tornado2.Init(playerDir, disableDuration, speed, scale);
                        break;
                    }

                    Vector2 dir = (playerTransform.position - transform.position).normalized;
                    float defaultAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                    float angle = (maxAngle / attackCount * (i + 1)) + defaultAngle;
                    int minus = attackCount / 2 + 1;
                    float finalAngle = angle - (minus * (maxAngle / attackCount));

                    Vector2 reflectBallDir = new Vector2(Mathf.Cos(finalAngle * Mathf.Deg2Rad),
                            Mathf.Sin(finalAngle * Mathf.Deg2Rad));

                    reflectBallDir.Normalize();

                    var tornado = pool.Get().GetComponent<Tornado>().GetComponent<Tornado>();

                    tornado.transform.position = transform.position;
                    tornado.Init(reflectBallDir, disableDuration, speed, scale);
                }
            }

        };

        if (currentSkillCount + 1 > SkillCount)
        {
            IsChange = true;
            return;
        }
    }

    public void CirlcePosition(int index, float randomCount)
    {
        float angle = ((360 / randomCount) * index) * Mathf.Deg2Rad;

        Vector2 CirclePos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
        if (pool == null) return;

        var tornado = pool.Get().GetComponent<Tornado>().GetComponent<Tornado>();
        tornado.transform.position = transform.position;
        tornado.Init(CirclePos, disableDuration, speed, scale);
    }

}
