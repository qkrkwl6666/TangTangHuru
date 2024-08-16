using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class TreasurePointer : MonoBehaviour
{
    private GameObject pivot;
    private Treasure targetTreasure;

    private void FixedUpdate()
    {
        if(targetTreasure != null)
        {
            SetDirection();

            if (!targetTreasure.gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void SetTarget(Treasure treasure)
    {
        targetTreasure = treasure;
    }
    public void SetPivot(GameObject obj)
    {
        pivot = obj;
    }

    private void SetDirection()
    {
        var dir = targetTreasure.transform.position - pivot.transform.position;
        transform.position = pivot.transform.position + (dir.normalized * 2f);
        transform.up = dir.normalized;
    }
}
