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

        if ((AttackableLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            if (!other.gameObject.activeSelf)
                return;

            if (Random.Range(0, 100) <= CriticalChance)
            {
                TotalDamage = Damage * CriticalValue;
            }
            else
            {
                TotalDamage = Damage;
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
