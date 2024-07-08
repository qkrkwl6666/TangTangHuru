using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAim : RangeDetecter, IAimer
{
    GameObject player;

    public GameObject Player { get => player; }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public Vector3 AimDirection()
    {
        return GetNearest();
    }

}
