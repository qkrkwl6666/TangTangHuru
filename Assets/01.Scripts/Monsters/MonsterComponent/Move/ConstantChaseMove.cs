using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ConstantChaseMove : MonoBehaviour
{
    private Monster monster;

    private void Awake()
    {
        monster = GetComponent<Monster>();
    }

    public void Update()
    {
        if (monster.Player == null) return;

        Vector2 dir = (monster.Player.transform.position - gameObject.transform.position).normalized;
        transform.Translate(dir * Time.deltaTime * monster.moveSpeed);
    }
}
