using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAimer
{
    GameObject Player { get; }

    Vector3 AimDirection();
}
