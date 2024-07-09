using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class FixedAim : MonoBehaviour, IAimer
{
    PlayerController controller;
    
    float dir;

    public GameObject player;
    public GameObject Player { get => player; }
    public float LifeTime { get; set; }
    public float Speed { get; set; }


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<PlayerController>();
    }

    public Vector3 AimDirection()
    {
        dir = controller.joystick.InputValue.x > 0 ? 1f : -1f;

        Vector3 aimPosition = Vector3.zero;
        aimPosition.x += dir;
        return aimPosition;
    }
}
