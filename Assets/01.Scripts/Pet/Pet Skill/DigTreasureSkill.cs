using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigTreasureSkill : MonoBehaviour
{
    public float checkRadius = 15f;
    public bool isEvolved = false;

    private float checkInterval = 3f;
    private PetController petController;

    private void Start()
    {
        petController = GetComponent<PetController>();
        InvokeRepeating(nameof(CheckNearbyObjects), 0f, checkInterval);
    }

    private void CheckNearbyObjects()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius);

        foreach (var hitCollider in hitColliders)
        {
            Treasure box = hitCollider.GetComponent<Treasure>();

            if (box != null)
            {
                petController.SetTargetPosition(box.transform.position);
            }
        }
    }
}
