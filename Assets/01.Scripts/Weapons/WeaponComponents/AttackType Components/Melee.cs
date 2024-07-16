using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public float range = 2f;

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
            dir = prevDir; //입력 없으면 이전 조준값 쓰기
        }
        else
        {
            prevDir = dir;
        }

        if(currAimer.AimDirection() == currAimer.Player.transform.position)
        {
            dir = Vector3.zero; //방향이 플레이어 위치면 dir 0으로 설정. 중심에서 따라다니는 스킬
        }

        dir *= range;
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
