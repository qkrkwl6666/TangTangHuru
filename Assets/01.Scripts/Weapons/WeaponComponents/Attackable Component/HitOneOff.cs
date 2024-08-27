using System.Collections.Generic;
using UnityEngine;

public class HitOneOff : MonoBehaviour, IAttackable
{
    public LayerMask AttackableLayer { get; set; }
    public float Damage { get; set; }
    public float PierceCount { get; set; }
    public float CriticalChance { get; set; }
    public float CriticalValue { get; set; }
    public float TotalDamage { get; set; }
    public float AttackRate { get; set; }
    public float Impact { get; set; }

    private HashSet<Collider2D> contactedEnemies = new HashSet<Collider2D>();

    private void OnEnable()
    {
        contactedEnemies.Clear();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.activeSelf && !contactedEnemies.Contains(other))
        {
            OnAttack(other);
            contactedEnemies.Add(other);
        }
    }

    public void OnAttack(Collider2D other)
    {
        var pierce = PierceCount;


        if ((AttackableLayer.value & (1 << other.gameObject.layer)) != 0
            || other.gameObject.layer == LayerMask.NameToLayer("Guardian"))
        {

            if (Random.Range(0, 100) <= CriticalChance)
            {
                TotalDamage = Damage * CriticalValue;
                other.GetComponent<DamageText>().isCritical = true;
                if (SoundManager.Instance.hitCoolTime <= 0f)
                {
                    SoundManager.Instance.PlayShortSound("Hit_crit", 0, false, SoundType.HIT_EFFECT);
                    SoundManager.Instance.HitCoolTimeOn();
                }
            }
            else
            {
                TotalDamage = Damage;
                if (SoundManager.Instance.hitCoolTime <= 0f)
                {
                    SoundManager.Instance.PlayShortSound("Hit_normal", 0, false, SoundType.HIT_EFFECT);
                    SoundManager.Instance.HitCoolTimeOn();
                }
            }
            other.gameObject.GetComponentInParent<IDamagable>().OnDamage(TotalDamage, Impact);

            if (pierce > 0)
            {
                pierce--;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

}

