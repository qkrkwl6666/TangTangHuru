using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public float lifeTime = 0f;
    float timer = 0f;

    IAimer currAimer;
    Vector3 dir;

    void Awake()
    {
        currAimer = GetComponent<IAimer>();
    }

    private void OnEnable()
    {
        dir = currAimer.AimDirection();
        dir *= 1.5f;
    }

    private void OnDisable()
    {
        timer = 0f;

    }

    private void Update()
    {
        if (timer > lifeTime)
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
