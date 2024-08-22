using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Boom : MonoBehaviour
{
    private IObjectPool<GameObject> pool;

    public ParticleSystem circle;
    public ParticleSystem impact;

    private float duration = 2f;
    private float time = 0f;

    private bool attack = false;
    private float damage = 1f;
    private bool isFirst = false;


    private void OnEnable()
    {
        gameObject.transform.localScale = Vector3.zero;
        time = 0f;
        attack = false;
        isFirst = false;
        circle.Play();
    }

    public void SetObjectPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }

    private void Awake()
    {
        var main = impact.main;
        main.loop = false;
    }

    private void Update()
    {
        time += Time.deltaTime;

        float scale = Mathf.InverseLerp(0f, duration, time);

        transform.localScale = new Vector3(scale, scale, scale);

        if (time >= duration && !isFirst)
        {
            attack = true;
            isFirst = true;

            StartCoroutine(CoAttackDisable());
        }
    }

    public void Initialize(float duration, float damage)
    {
        this.duration = duration;
        this.damage = damage;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!attack) return;

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<IDamagable>().OnDamage(damage, 0);
            pool.Release(gameObject);
            return;
        }
    }

    public IEnumerator CoAttackDisable()
    {
        impact.Play();

        yield return new WaitForSeconds(0.15f);

        pool.Release(gameObject);
    }
}
