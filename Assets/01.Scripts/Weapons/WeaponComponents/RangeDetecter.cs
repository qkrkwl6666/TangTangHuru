using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDetecter : MonoBehaviour
{
    public float range = 10f;
    public LayerMask targetLayer;
    RaycastHit2D[] targets;

    public Transform nearest;

    public void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, range, Vector2.zero, 0, targetLayer);
        nearest = GetNearest();
    }

    private Transform GetNearest()
    {
        float currdistance = float.MaxValue;
        Transform result = null;
        foreach (var target in targets)
        {
            var distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance < currdistance)
            {
                currdistance = distance;
                result = target.transform;
            }
        }
        return result;
    }

}
