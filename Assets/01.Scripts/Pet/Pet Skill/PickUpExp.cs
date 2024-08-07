using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpExp : MonoBehaviour
{
    public float checkRadius = 15f;
    public float checkInterval = 30f;

    private void Start()
    {
        InvokeRepeating(nameof(CheckNearbyObjects), 0f, checkInterval);
    }

    private void CheckNearbyObjects()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius);

        foreach (var hitCollider in hitColliders)
        {
            MonsterExp monsterExp = hitCollider.GetComponent<MonsterExp>();

            if (monsterExp != null)
            {
                monsterExp.SetTarget(transform);
            }
        }
    }
}
