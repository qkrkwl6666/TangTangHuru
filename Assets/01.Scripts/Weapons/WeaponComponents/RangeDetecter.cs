using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDetecter : MonoBehaviour
{
    private float range = 30f;
    public LayerMask targetLayer;

    public Vector3 GetNearest()
    {
         var targets = Physics2D.CircleCastAll(transform.position, range, Vector2.zero, 0, targetLayer);

        float currdistance = float.MaxValue;
        Vector3 result = Vector3.zero;
        foreach (var target in targets)
        {
            var distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance < currdistance)
            {
                currdistance = distance;
                result = target.transform.position;
            }
        }
        return result.normalized;
    }

}
