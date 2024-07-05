using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AutoTargeting : MonoBehaviour
{
    public WeaponInfo weaponInfo;

    private Rigidbody2D rb;
    private RangeDetecter detector;
    private float timer = 0f;
    
    Vector3 dir = Vector3.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        detector = GetComponent<RangeDetecter>();
    }

    private void OnEnable()
    {
        var targetTrans = detector.GetNearest();

        if (targetTrans != null)
        {
            dir = targetTrans.position - transform.position;
        }
        else
        {
            dir = Random.insideUnitCircle;
        }
        rb.velocity = dir.normalized * weaponInfo.weapon_Speed;
    }

    private void OnDisable()
    {
        timer = 0f;
    }

    private void Update()
    {
        if(timer > weaponInfo.weapon_LifeTime)
        {
            timer = 0f;
            gameObject.SetActive(false);
        }
        else
        {
            timer += Time.deltaTime;
        }


    }
}
