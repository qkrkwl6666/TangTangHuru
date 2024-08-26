using UnityEngine;

public class HitOnEnter : MonoBehaviour, IAttackable
{
    public LayerMask AttackableLayer { get; set; }
    public float Damage { get; set; } = 10f;
    public float PierceCount { get; set; } = 0f;
    public float CriticalChance { get; set; } = 10f;
    public float CriticalValue { get; set; } = 10f;
    public float TotalDamage { get; set; } = 10f;
    public float AttackRate { get; set; } = 0f;
    public float Impact { get; set; } = 0f;


    private void OnTriggerEnter2D(Collider2D other)
    {
        OnAttack(other);
    }

    public void OnAttack(Collider2D other)
    {
        AttackableLayer = LayerMask.GetMask("Enemy");

        var pierce = PierceCount;

        if ((AttackableLayer.value & (1 << other.gameObject.layer)) != 0
            || other.gameObject.layer == LayerMask.NameToLayer("Guardian"))
        {
            if (!other.gameObject.activeSelf)
                return;

            if (Random.Range(0, 100) <= CriticalChance)
            {
                TotalDamage = Damage * CriticalValue;
                other.GetComponent<DamageText>().isCritical = true;

                if(SoundManager.Instance.hitCoolTime <= 0f)
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
