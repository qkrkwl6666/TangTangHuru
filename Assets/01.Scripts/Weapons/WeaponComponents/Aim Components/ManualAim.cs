using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualAim : MonoBehaviour, IAimer
{
    GameObject player;
    PlayerController controller;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<PlayerController>();
    }

    public Vector3 AimDirection()
    {
        Vector3 aimPosition = player.transform.position + (Vector3)controller.joystick.InputValue;
        return aimPosition;
    }
}
