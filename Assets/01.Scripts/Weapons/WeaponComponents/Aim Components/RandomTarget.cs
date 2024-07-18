using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTarget : MonoBehaviour, IAimer
{
    public GameObject player;
    public GameObject Player { get => player; }
    public float LifeTime { get; set; }
    public float Speed { get; set; }
    public int TotalCount { get; set; }
    public int Index { get; set; }

    private float range = 30f;
    
    public LayerMask targetLayer = LayerMask.GetMask("Enemy");

    public Vector3 AimDirection()
    {
        var targets = Physics2D.CircleCastAll(transform.position, range, Vector2.zero, 0, targetLayer);

        var index = Random.Range(0, targets.Length);

        Vector3 result = targets[index].transform.position;

        return result;
    }
}
