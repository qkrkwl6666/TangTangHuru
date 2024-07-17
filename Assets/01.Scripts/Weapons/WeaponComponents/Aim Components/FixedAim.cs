using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class FixedAim : MonoBehaviour, IAimer
{
    PlayerController controller;
    public GameObject player;
    public GameObject Player { get => player; }
    public float LifeTime { get; set; }
    public float Speed { get; set; }
    public int TotalCount { get; set; }
    public int Index {get; set; }

    private float dir;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<PlayerController>();
    }

    public Vector3 AimDirection()
    {
        dir = (Index % 2) == 0 ? -1f : 1f;

        Vector3 aimPosition = Vector3.zero;
        aimPosition.x += dir;
        return aimPosition;
    }
}
