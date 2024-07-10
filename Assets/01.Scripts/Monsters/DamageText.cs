using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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
