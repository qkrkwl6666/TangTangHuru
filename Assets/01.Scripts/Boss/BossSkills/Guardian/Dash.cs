using Spine;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

// 가디언 전용 스킬
public class Dash : MonoBehaviour, IBossSkill
{
    public int SkillCount { get; set; } = 1;
    public bool IsChange { get; set; } = false;
    public float SkillRate { get ; set ; } = 1f;
    public float DamageFactor { get; set; } = 1f;
    public float Damage { get; set; } = 1f;

    private float dashSpeed = 10f;

    private float dashTime = 2f;

    private BossView bossView;
    private Boss boss;

    private float time = 0f;

    private bool isAttacking = false;

    private Vector2 PlayerDir = Vector2.zero;

    private void Awake()
    {
        boss = GetComponent<Boss>();
        bossView = GetComponentInChildren<BossView>();
    }

    public void Activate()
    {
        enabled = true;

        PlayerDir = (boss.PlayerTransform.position - transform.position).normalized;
    }

    public void DeActivate()
    {
        enabled = false;
        IsChange = false;
        isAttacking = false;
        time = 0f;
        PlayerDir = Vector2.zero;
    }

    public void Initialize(BossSkillData bossSkillData, float damage)
    {
        SkillCount = bossSkillData.Skill_Count;
        DamageFactor = bossSkillData.Damage_Factor;
        SkillRate = bossSkillData.Skill_Rate;
        Damage = damage * DamageFactor;
        enabled = false;
    }

    public void SkillUpdate(float deltaTime)
    {
        time += deltaTime;

        if(time >= dashTime)
        {
            boss.ChangeState(boss.walkState);
        }

        if (!isAttacking && PlayerDir != Vector2.zero)
        {
            transform.Translate(PlayerDir * dashSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isAttacking) return;

        if (collision.gameObject.tag == "Player")
        {

            bossView.PlayAnimation(Defines.attack, false).Complete += (trackEntry) => 
            {
                collision.gameObject.GetComponent<IDamagable>().OnDamage(Damage, 0);
                IsChange = true;
            };

            return;
        }

    }
}
