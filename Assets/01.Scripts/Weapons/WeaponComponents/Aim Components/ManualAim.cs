using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualAim : MonoBehaviour, IAimer
{
    public GameObject player;
    PlayerController controller;

    public GameObject Player { get => player; }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<PlayerController>();
    }

    public Vector3 AimDirection()
    {
        Vector3 aimPosition = (Vector3)controller.joystick.InputValue;
        return aimPosition.normalized;
    }
}
