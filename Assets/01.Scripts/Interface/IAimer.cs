using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAimer
{
    GameObject Player { get; }

    float LifeTime { get; set; }
    float Speed { get; set; }
    int TotalCount { get; set; }
    int Index {  get; set; }

    Vector3 AimDirection();
}
