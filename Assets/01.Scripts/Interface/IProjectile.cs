using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    float Range { get; set; }
    float Size { get; set; }
    float Speed { get; set; }
}
