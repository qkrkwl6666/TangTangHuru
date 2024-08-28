using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterBoom : MonoBehaviour
{
    private IObjectPool<GameObject> pool;

    private ParticleSystem ps;

    public float disableDuration = 2f;
    private float boomTime = 0.2f;

    private bool isAttackable = false;
    private float radius = 2f;
    private float damage = 50f;
    private float time = 0f;

    private int layerPlayer;
    private int layerMonster;
    private int totalLayer;

    private bool isBoom = false;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        layerPlayer = LayerMask.NameToLayer(Defines.player);
        layerMonster = LayerMask.NameToLayer(Defines.enemy);

        totalLayer = 1 << layerPlayer | 1 << layerMonster;
    }

    public void Init(float damage)
    {
        this.damage = damage;
    }

    private void OnEnable()
    {
        ps.Play();
    }

    private void Update()
    {
        time += Time.deltaTime;

        if(time >= boomTime && !isBoom)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);

            foreach (var hitCollider in hitColliders)
            {
                if(hitCollider.CompareTag("Player") || hitCollider.gameObject.layer == layerMonster)
                {
                    hitCollider.GetComponent<LivingEntity>().OnDamage(damage, 5f);
                }
            }

            isBoom = true;
        }

        if (time >= disableDuration) 
        {
            pool.Release(gameObject);
        }
    }

    private void OnDisable()
    {
        time = 0f;
        isBoom = false;
        ps.Stop();
    }

    public void SetObjectPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }

}
