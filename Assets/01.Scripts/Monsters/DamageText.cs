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

        //if(textObjects.Count < 5)
        //{
        //    var newText = Instantiate(textObject, gameObject.transform.position, Quaternion.identity);
        //    newText.GetComponent<TextMeshPro>().text = (damage / 1).ToString();
        //    textObjects.Add(newText);
        //    return;
        //}

        //foreach (var text in textObjects)
        //{
        //    if (!text.gameObject.activeSelf)
        //    {
        //        text.gameObject.SetActive(true);
        //        text.gameObject.transform.position = transform.position + textPos;
        //        text.GetComponent<TextMeshPro>().text = (damage / 1).ToString();
        //        return;
        //    }
        //}
    }
}
