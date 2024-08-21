using UnityEngine;

public class DamageText : MonoBehaviour
{
    private LivingEntity monster;

    private bool isTextOn = true;
    public bool isCritical = false;

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

        MonsterManager.Instance.ShowDamage((int)damage, transform.position, isCritical);
        isCritical = false;
    }

    public void ActiveDamageText(bool isOn)
    {
        isTextOn = isOn;
    }
}
