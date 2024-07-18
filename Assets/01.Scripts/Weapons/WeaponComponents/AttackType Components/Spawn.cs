using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public float range = 2f;

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

        if (dir == Vector3.zero)
        {
            dir = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0);
        }

        dir *= (range + 1);

        transform.position = currAimer.Player.transform.position + dir;
        transform.up = dir;
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
            timer += Time.deltaTime;
        }
    }
}
