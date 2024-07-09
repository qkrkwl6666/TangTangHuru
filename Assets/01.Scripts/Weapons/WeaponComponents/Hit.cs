using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public GameObject hitText;
    public GameObject[] hitTexts;

    public LayerMask attackableLayer;

    public float damage;
    public float pierceCount;

    public float criticalChance;
    public float criticalValue;

    private float totalDamage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnAttack(other);
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

        //foreach(var hit in hitTexts)
        //{
        //    if (!hit.activeSelf)
        //    {

        //    }
        //}
        
    }
}
