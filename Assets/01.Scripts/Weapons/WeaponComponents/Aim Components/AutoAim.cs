using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAim : RangeDetecter, IAimer
{
    public Vector3 AimDirection()
    {
        return GetNearest();
    }

}
