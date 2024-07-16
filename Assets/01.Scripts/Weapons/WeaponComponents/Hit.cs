using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Hit : MonoBehaviour
{
    public LayerMask attackableLayer;

    public float damage;
    public float pierceCount;

    public float criticalChance;
    public float criticalValue;

    private float totalDamage;

    public bool one_Off = false;
    private HashSet<Collider2D> contactedEnemies;

    void Start()
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
        var pierce = pierceCount;

        if ((attackableLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            if(Random.Range(0, 100) <= criticalChance)
            {
                totalDamage = damage * criticalValue;
            }
            else
            {
                totalDamage = damage;
            }

            other.gameObject.GetComponent<IDamagable>().OnDamage(totalDamage);

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
