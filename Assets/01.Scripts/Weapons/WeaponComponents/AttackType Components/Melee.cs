using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    float timer = 0f;

    IAimer currAimer;
    Vector3 dir;

    void Awake()
    {
        currAimer = GetComponent<IAimer>();
    }

    private void OnEnable()
    {
        if (currAimer == null)
            return;
        dir = currAimer.AimDirection();
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
            timer += Time.deltaTime;
        }
    }
}
