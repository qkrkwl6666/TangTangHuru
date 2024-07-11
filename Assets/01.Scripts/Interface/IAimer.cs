using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAimer
{
    GameObject Player { get; }

    float LifeTime { get; set; }
    float Speed { get; set; }
    int Count { get; set; }

    Vector3 AimDirection();
}
