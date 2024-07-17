using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour, IAttackable
{
    public LayerMask AttackableLayer { get; set; }
    public float Damage { get; set; }
    public float PierceCount { get; set; }
    public float CriticalChance { get; set; }
    public float CriticalValue { get; set; }
    public float TotalDamage { get; set; }

    public bool one_Off = false;
    private HashSet<Collider2D> contactedEnemies;

    private void Start()
    {
        contactedEnemies = new HashSet<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!one_Off)
        {
            OnAttack(other);
            return;
        }

        if (!contactedEnemies.Contains(other))
        {
            OnAttack(other);
            contactedEnemies.Add(other);
        }
    }

    public void OnAttack(Collider2D other)
    {
        var pierce = PierceCount;

        if ((AttackableLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            if(Random.Range(0, 100) <= CriticalChance)
            {
                TotalDamage = Damage * CriticalValue;
            }
            else
            {
                TotalDamage = Damage;
            }

            other.gameObject.GetComponent<IDamagable>().OnDamage(TotalDamage);

            if(pierce > 0)
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
