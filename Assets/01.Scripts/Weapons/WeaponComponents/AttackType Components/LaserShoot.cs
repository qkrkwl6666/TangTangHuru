using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShoot : MonoBehaviour
{
    private LineRenderer laser;
    IAimer currAimer;
    Vector3 dir;

    void Awake()
    {
        currAimer = GetComponent<IAimer>();
    }
    void Start()
    {
        laser = GetComponent<LineRenderer>();
        laser.SetPosition(0, currAimer.Player.transform.position);
        laser.SetPosition(1, currAimer.AimDirection() * 10f);
    }

    // Update is called once per frame
    void Update()
    {
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, currAimer.AimDirection() * 10f);
    }
}
