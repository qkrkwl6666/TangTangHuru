using UnityEngine;

public class DamageText : MonoBehaviour
{
    private Monster monster;

    private void Awake()
    {
        monster = GetComponent<Monster>();
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
