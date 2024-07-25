using UnityEngine;

public class SlowOnHit : MonoBehaviour
{
    public LayerMask AttackableLayer { get; set; }
    public float AttackRate { get; set; }

    private float timer = 0f;
    private bool attackReady = true;

    private Collider2D triggerCollider;


    private void Awake()
    {
        triggerCollider = GetComponent<Collider2D>();
        AttackableLayer = LayerMask.GetMask("Enemy");
        AttackRate = 1f;
    }

    private void Update()
    {
        if (attackReady)
        {
            AttackReady();
        }

        if (timer > AttackRate)
        {
            attackReady = true;
            timer = 0f;
        }
        else
        {
            timer += Time.deltaTime;
        }

    }

    private void AttackReady()
    {
        //트리거에 닿아있는 모든 콜라이더 가져와 OnAttack() 함수의 매개변수로 넣음

        Collider2D[] hitColliders = new Collider2D[100];
        ContactFilter2D contactFilter = new ContactFilter2D();
        int numColliders = triggerCollider.OverlapCollider(contactFilter, hitColliders);

        if (numColliders > 0)
        {
            Collider2D[] collidersToAttack = new Collider2D[numColliders];
            System.Array.Copy(hitColliders, collidersToAttack, numColliders);
            foreach (var collider in collidersToAttack)
            {
                if (!collider.enabled)
                    continue;

                if ((AttackableLayer.value & (1 << collider.gameObject.layer)) != 0)
                {
                    OnSlow(collider);
                }
            }
        }
        attackReady = false;
    }

    public void OnSlow(Collider2D other)
    {
        var controller = other.gameObject.GetComponent<MonsterController>();
        if (controller != null)
        {
            controller.Slow(1);
        }
    }
}
