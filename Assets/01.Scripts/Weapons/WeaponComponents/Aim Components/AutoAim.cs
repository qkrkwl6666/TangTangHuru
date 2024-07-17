using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAim : RangeDetecter, IAimer
{
    GameObject player;
    public GameObject Player { get => player; }
    public float LifeTime { get; set; }
    public float Speed { get; set; }
    public int TotalCount { get; set; }
    public int Index { get; set; }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        targetLayer = LayerMask.GetMask("Enemy");
    }

    public Vector3 AimDirection()
    {
        var getPos = GetNearest();
        if (getPos == Vector3.zero)
        {
            getPos = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        }
        return getPos.normalized;
    }

}
