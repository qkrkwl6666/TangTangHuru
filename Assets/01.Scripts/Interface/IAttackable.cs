using UnityEngine;

public interface IAttackable
{
    public LayerMask AttackableLayer { get; set; }

    public float Damage { get; set; }
    public float PierceCount { get; set; }
    public float CriticalChance { get; set; }
    public float CriticalValue { get; set; }
    public float TotalDamage { get; set; }
    public float AttackRate {  get; set; }

    void OnAttack(Collider2D other);

}
