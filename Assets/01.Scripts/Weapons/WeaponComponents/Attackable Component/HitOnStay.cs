using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitOnStay : MonoBehaviour, IAttackable
{
    public LayerMask AttackableLayer { get; set; }
    public float Damage { get; set; }
    public float PierceCount { get; set; }
    public float CriticalChance { get; set; }
    public float CriticalValue { get; set; }
    public float TotalDamage { get; set; }

    public bool one_Off = false;
    private HashSet<Collider2D> contactedEnemies;

    private float attackRate = 0.3f;
    private float timer = 0f;
    private bool attackReady = true;

    private void Start()
    {
        contactedEnemies = new HashSet<Collider2D>();
    }

    private void Update()
    {
        if (attackReady)
            return;

        if (timer > attackRate)
        {
            attackReady = true;
            timer = 0f;
        }
        else
        {
            timer += Time.deltaTime;
        }

    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!attackReady)
            return;

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
            attackReady = false;

            if (Random.Range(0, 100) <= CriticalChance)
            {
                TotalDamage = Damage * CriticalValue;
            }
            else
            {
                TotalDamage = Damage;
            }

            other.gameObject.GetComponent<IDamagable>().OnDamage(TotalDamage);

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
