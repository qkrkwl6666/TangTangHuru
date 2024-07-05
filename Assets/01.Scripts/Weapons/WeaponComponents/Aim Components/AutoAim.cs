using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAim : RangeDetecter, IAimer
{
    public Transform AimDirection()
    {
        return GetNearest();
    }

}
