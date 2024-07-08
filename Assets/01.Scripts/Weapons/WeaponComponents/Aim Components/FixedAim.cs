using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class FixedAim : MonoBehaviour, IAimer
{
    GameObject player;
    PlayerController controller;
    float dir;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<PlayerController>();
    }
    public Vector3 AimDirection()
    {
        dir = controller.joystick.InputValue.x > 0 ? 3f : -3f;

        Vector3 aimPosition = player.transform.position;
        aimPosition.x += dir;
        return aimPosition;
    }
}
