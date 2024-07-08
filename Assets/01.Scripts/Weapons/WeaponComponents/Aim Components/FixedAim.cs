using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedAim : MonoBehaviour, IAimer
{
    GameObject player;
    Vector3 left;
    Vector3 right;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        left = player.transform.position;
        left.x -= 3f;
        right = player.transform.position;
        right.y += 3f;
    }
    public Vector3 AimDirection()
    {
        left = player.transform.position;
        left.x -= 3f;
        return left;
    }
}
