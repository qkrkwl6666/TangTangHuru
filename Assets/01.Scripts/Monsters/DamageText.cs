using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public GameObject textObject;
    private List<GameObject> textObjects = new();
    private Vector3 textPos = new Vector3(0, 2, 0);

    private Monster monster;

    private void Awake()
    {
        monster = GetComponent<Monster>();
        monster.OnDamaged += ShowDamage;
    }

    private void OnDestroy()
    {
        monster.OnDamaged -= ShowDamage;
    }

    private void ShowDamage(float damage)
    {
        bool popped = false;
        foreach (var text in textObjects)
        {
            if (!text.gameObject.activeSelf)
            {
                text.gameObject.SetActive(true);
                text.gameObject.transform.position = transform.position + textPos;
                text.GetComponent<TextMeshPro>().text = (damage / 1).ToString();

                popped = true;
                break;
            }
        }

        if (!popped)
        {
            var newText = Instantiate(textObject, gameObject.transform.position, Quaternion.identity);
            newText.GetComponent<TextMeshPro>().text = (damage / 1).ToString();
            textObjects.Add(newText);
            popped = true;
        }
    }
}
