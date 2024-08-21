using UnityEngine;

public class DamageText : MonoBehaviour
{
    private LivingEntity monster;

    private bool isTextOn = true;

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
        if (!isTextOn)
            return;

        MonsterManager.Instance.ShowDamage(damage, transform.position);
    }

    public void ActiveDamageText(bool isOn)
    {
        isTextOn = isOn;
    }
}
