using System.Collections;
using UnityEngine;

public class StunCircle : MonoBehaviour
{
    public ParticleSystem impact;

    private float duration = 3f;
    private float time = 0f;

    private bool attack = false;
    private float damage = 1f;
    private bool isFirst = false;

    private float stunDuration = 0.5f;
    private float circleScale = 10f;

    private Boss boss;
    private BossView bossView;

    private void OnEnable()
    {
        gameObject.transform.localScale = Vector3.zero;
        time = 0f;
        attack = false;
        isFirst = false;
    }

    public void Initialize(Boss boss, BossView bossView, float duration, float damage)
    {
        this.boss = boss;
        this.bossView = bossView;
        this.duration = duration;
        this.damage = damage;
    }

    private void Update()
    {
        time += Time.deltaTime;

        float scale = Mathf.InverseLerp(0f, duration, time) * circleScale;

        transform.localScale = new Vector3(scale, scale, scale);

        if (time >= duration && !attack)
        {
            attack = true;

            StartCoroutine(CoChangeState());
        }
    }

    public IEnumerator CoChangeState()
    {
        yield return new WaitForSeconds(0.15f);

        if (isFirst) yield break;

        boss.ChangeState(boss.walkState);
        gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!attack || isFirst) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            isFirst = true;

            bossView.PlayAnimation(Defines.attack, false).Complete += (x) =>
            {
                collision.GetComponent<PlayerController>
                    ().StartStun(stunDuration);

                boss.ChangeState(boss.walkState);

                gameObject.SetActive(false);
            };

            return;
        }
    }
}
