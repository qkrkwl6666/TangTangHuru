using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAim : RangeDetecter, IAimer
{
    GameObject player;
    public GameObject Player { get => player; }
    public float LifeTime { get; set; }
    public float Speed { get; set; }
    public int Count { get; set; }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        targetLayer = LayerMask.GetMask("Enemy");
    }

    public Vector3 AimDirection()
    {
        return GetNearest();
    }

}
