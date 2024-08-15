using UnityEngine;

public class PickUpExp : MonoBehaviour
{
    public float checkRadius = 15f;
    private float checkInterval = 20f;

    public bool isEvolved = false;
    private void Start()
    {
        if (isEvolved)
        {
            checkInterval = 10f;
        }
        else
        {
            checkInterval = 20f;
        }

        //InvokeRepeating(nameof(CheckNearbyObjects), 0f, checkInterval);
    }

    //private void CheckNearbyObjects()
    //{
    //    Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius);

    //    foreach (var hitCollider in hitColliders)
    //    {
    //        MonsterExp monsterExp = hitCollider.GetComponent<MonsterExp>();

    //        if (monsterExp != null)
    //        {
    //            monsterExp.SetTarget(transform);
    //        }
    //    }
    //}
}
