using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngularAim : MonoBehaviour, IAimer
{
    GameObject player;
    public GameObject Player { get => player; }

    public float LifeTime { get; set; }
    public float Speed { get; set; }
    public int TotalCount { get; set; }
    public int Index { get; set; }

    float angle = 0f;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public Vector3 AimDirection()
    {
        float angle = (360f / TotalCount) * Index;
        angle *= Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        return direction;
    }
}
