using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Rush : MonoBehaviour, IBossSkill
{
    public float Damage { get; set; } = 10f;
    public int SkillCount { get; set; } = 1;
    public bool IsChange { get; set; } = false;
    public float SkillRate { get; set; } = 1.5f;
    public float DamageFactor { get; set; } = 5f;

    private float dashRate = 0f;

    private float moveFactor = 5f;
    private float moveSpeed = 0f;

    private int currentSkillCount = 0;

    private Vector2 PlayerDir = Vector2.zero;
    private float time = 0f;
    private Boss boss;
    private bool isRush = false;

    private MonsterView monsterView;

    private void Awake()
    {
        boss = GetComponent<Boss>();
        monsterView = GetComponentInChildren<MonsterView>();
    }

    public void Activate()
    {
        enabled = true;
        dashRate = SkillRate;
    }

    public void DeActivate()
    {
        time = 0f;
        enabled = false;
        currentSkillCount = 0;
        IsChange = false;
        isRush = false;
        PlayerDir = Vector2.zero;

        monsterView.skeletonAnimation.skeleton.R = 1f;
        monsterView.skeletonAnimation.skeleton.G = 1f;
        monsterView.skeletonAnimation.skeleton.B = 1f;
    }

    public void Initialize(BossSkillData bossSkillData, float damage)
    {
        SkillCount = bossSkillData.Skill_Count;
        DamageFactor = bossSkillData.Damage_Factor;
        SkillRate = bossSkillData.Skill_Rate;
        Damage = damage * DamageFactor;
        enabled = false;
        moveSpeed = boss.Speed * moveFactor;
    }

    public void Attack()
    {
        currentSkillCount++;
        isRush = true;
        PlayerDir = (boss.PlayerTransform.position - transform.position).normalized;
        dashRate /= 2;
        monsterView.PlayAnimation(Defines.walk);
    }

    public void SkillUpdate(float deltaTime)
    {
        time += deltaTime;

        if (!isRush)
        {
            float r = Mathf.InverseLerp(0f, SkillRate, time);
            monsterView.skeletonAnimation.skeleton.R = r;
            monsterView.skeletonAnimation.skeleton.G = 0.2f;
            monsterView.skeletonAnimation.skeleton.B = 0.2f;
        }

        if (time >= dashRate && !isRush)
        {
            Debug.Log(dashRate);
            Attack();
        }

        if(isRush && PlayerDir != Vector2.zero)
        {
            transform.Translate(PlayerDir * moveSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isRush) return;

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<IDamagable>().OnDamage(Damage);
            IsChange = true;

            AwakeColor();
            return;
        }

        // Todo : 애니메이션이 작동중이면 멈춰야함
        monsterView.PlayAnimation(Defines.idle, true);
        if (currentSkillCount >= SkillCount)
        {
            IsChange = true;
        }

        time = 0f;
        isRush = false;
        PlayerDir = Vector2.zero;

        AwakeColor();
    }

    public void AwakeColor()
    {
        monsterView.skeletonAnimation.skeleton.R = 1f;
        monsterView.skeletonAnimation.skeleton.G = 1f;
        monsterView.skeletonAnimation.skeleton.B = 1f;
    }

}
