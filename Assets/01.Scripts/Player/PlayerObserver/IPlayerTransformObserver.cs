using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerTransformObserver : IPlayerObserver
{
    public void SetPlayerTransform(Transform transform);
}
