using UnityEngine;

public class HitOnStay : MonoBehaviour, IAttackable
{
    public LayerMask AttackableLayer { get; set; }
    public float Damage { get; set; }
    public float PierceCount { get; set; }
    public float CriticalChance { get; set; }
    public float CriticalValue { get; set; }
    public float TotalDamage { get; set; }
    public float AttackRate { get; set; }
    public float Impact { get; set; }

    private float timer = 0f;
    private bool attackReady = true;

    private Collider2D triggerCollider;


    private void Awake()
    {
        triggerCollider = GetComponent<Collider2D>();

        //if (triggerCollider != null && !triggerCollider.isTrigger)
        //{
        //    Debug.LogError("Collider2D is not set as a trigger!");
        //}
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

                if ((AttackableLayer.value & (1 << collider.gameObject.layer)) != 0
                    || collider.gameObject.layer == LayerMask.NameToLayer("Guardian"))
                {
                    OnAttack(collider);
                }
            }
        }
        attackReady = false;
    }

    public void OnAttack(Collider2D other)
    {
        var pierce = PierceCount;

        if (Random.Range(0, 100) <= CriticalChance)
        {
            TotalDamage = Damage * CriticalValue;
            other.GetComponent<DamageText>().isCritical = true;
        }
        else
        {
            TotalDamage = Damage;
        }

        other.gameObject.GetComponent<IDamagable>().OnDamage(TotalDamage, Impact);

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
