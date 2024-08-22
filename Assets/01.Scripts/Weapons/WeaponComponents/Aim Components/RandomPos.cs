using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPos : MonoBehaviour, IAimer
{
    public GameObject player;
    public GameObject Player { get => player; }
    public float LifeTime { get; set; }
    public float Speed { get; set; }
    public int TotalCount { get; set; }
    public int Index { get; set; }

    private float range = 6f;

    public LayerMask targetLayer;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public Vector3 AimDirection()
    {
        Vector3 result = Vector3.zero;

        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(0f, range);
        result = (Vector2)player.transform.position + randomDirection * randomDistance;
        return result;
    }
}

