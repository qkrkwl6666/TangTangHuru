using UnityEngine;

public class MonsterBaseAttack : MonoBehaviour
{
    public LayerMask attackableLayer;

    public float damage;

    public float criticalChance;
    public float criticalValue;

    public float attackRate = 0.1f;

    private float totalDamage;

    private bool attackable = true;
    private float timer = 0f;

    private void Update()
    {
        if (timer > attackRate)
        {
            attackable = true;
            timer = 0f;
        }
        else
        {
            timer += Time.deltaTime;
        }

    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!attackable)
            return;

        OnAttack(other);
    }

    public void OnAttack(Collision2D other)
    {

        if ((attackableLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            if (Random.Range(0, 100) <= criticalChance)
            {
                totalDamage = damage * criticalValue;
            }
            else
            {
                totalDamage = damage;
            }

            other.gameObject.GetComponent<IDamagable>().OnDamage(totalDamage);

            attackable = false;
        }
    }
}
