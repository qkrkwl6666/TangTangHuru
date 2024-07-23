using UnityEngine;

public interface IPlayerTransformObserver : IPlayerObserver
{
    public void SetPlayerTransform(Transform transform);
}
