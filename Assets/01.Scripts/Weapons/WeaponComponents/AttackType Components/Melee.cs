using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    float timer = 0f;

    IAimer currAimer;
    Vector3 dir;
    Vector3 prevDir = Vector3.zero;

    void Awake()
    {
        currAimer = GetComponent<IAimer>();
        prevDir.x = 1f;
    }

    private void OnEnable()
    {
        if (currAimer == null)
            return;

        dir = currAimer.AimDirection();

        if(dir == Vector3.zero)
        {
            dir = prevDir;
        }
        else
        {
            prevDir = dir;
        }
        dir *= 1.3f;
    }

    private void OnDisable()
    {
        timer = 0f;
    }

    private void Update()
    {
        if (timer > currAimer.LifeTime)
        {
            gameObject.SetActive(false);
        }
        else
        {
            transform.position = currAimer.Player.transform.position + dir;
            transform.up = dir;
            timer += Time.deltaTime;
        }
    }
}
