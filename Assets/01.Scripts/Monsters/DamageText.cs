using UnityEngine;

public class DamageText : MonoBehaviour
{
    private LivingEntity monster;

    private void Awake()
    {
        monster = GetComponent<LivingEntity>();
        monster.OnDamaged += ShowHeadUpDamage;
    }

    private void OnDestroy()
    {
        monster.OnDamaged -= ShowHeadUpDamage;
    }

    private void ShowHeadUpDamage(float damage)
    {
        MonsterManager.Instance.ShowDamage(damage, transform.position);
    }
}
