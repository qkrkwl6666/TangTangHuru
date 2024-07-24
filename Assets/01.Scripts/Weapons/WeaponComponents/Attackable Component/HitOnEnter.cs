using UnityEngine;

public class HitOnEnter : MonoBehaviour, IAttackable
{
    public LayerMask AttackableLayer { get; set; }
    public float Damage { get; set; }
    public float PierceCount { get; set; }
    public float CriticalChance { get; set; }
    public float CriticalValue { get; set; }
    public float TotalDamage { get; set; }
    public float AttackRate { get; set; }


    private void OnTriggerEnter2D(Collider2D other)
    {
        OnAttack(other);
    }

    public void OnAttack(Collider2D other)
    {
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

            other.gameObject.GetComponentInParent<IDamagable>().OnDamage(TotalDamage);

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
