using UnityEngine;
using UnityEngine.AddressableAssets;

public class Stunned : MonoBehaviour, IBossSkill
{
    public int SkillCount { get; set; } = 1;
    public bool IsChange { get; set; } = false;
    public float SkillRate { get; set; } = 1f;
    public float DamageFactor { get; set; } = 1f;
    public float Damage { get; set; } = 1f;

    public GameObject stunCirlce;

    private Transform playerTransform;
    private BossView bossView;
    private Boss boss;

    private float time = 0f;
    private float circleScale = 10f;

    private void Awake()
    {
        boss = GetComponent<Boss>();
        bossView = GetComponentInChildren<BossView>();
    }

    public void Initialize(BossSkillData bossSkillData, float damage)
    {
        Addressables.InstantiateAsync(Defines.stunCirlce, transform).Completed
            += (x) =>
        {
            var go = x.Result;
            stunCirlce = go;
            stunCirlce.GetComponent<StunCircle>().Initialize(boss, bossView, SkillRate, damage);
            stunCirlce.SetActive(false);
        };

        SkillCount = bossSkillData.Skill_Count;
        DamageFactor = bossSkillData.Damage_Factor;
        SkillRate = bossSkillData.Skill_Rate;
        Damage = damage * DamageFactor;
        enabled = false;

        playerTransform = boss.PlayerTransform;
    }

    public void Activate()
    {
        enabled = true;

        Attack();
    }

    public void DeActivate()
    {
        time = 0f;
        enabled = false;
        IsChange = false;
    }

    public void SkillUpdate(float deltaTime)
    {
        time += deltaTime;

        //if (time >= SkillRate)
        //{
        //    time = 0f;
        //    Attack();
        //}
    }

    public void Attack()
    {
        stunCirlce.gameObject.SetActive(true);
    }
}
