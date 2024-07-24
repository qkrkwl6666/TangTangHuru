using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaRotate : MonoBehaviour, IProjectile
{
    public IAimer currAimer;

    private float timer = 0f;

    public float Range { get; set; }
    public float Size { get; set; }
    public float Speed { get; set; }

    public float angleOffset = 30f; // 타원이 기울어진 각도 (오브젝트 전진 방향 기준 우측으로)

    private Transform parentTransform;


    void Awake()
    {
        currAimer = GetComponent<IAimer>();
    }

    void Start()
    {
        parentTransform = currAimer.Player.transform;
    }

    private void OnEnable()
    {
        transform.localScale = new Vector3(Size, Size);
        timer = 0f;
    }

    private void OnDisable()
    {
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= currAimer.LifeTime)
        {
            timer = 0f;
            gameObject.SetActive(false);
        }

        float lerpTime = timer * Speed / 2;
        float cosAngle = Mathf.Cos(lerpTime);
        float sinAngle = Mathf.Sin(lerpTime);

        Vector3 newPosition = parentTransform.position + currAimer.AimDirection() * (cosAngle * Range) +
                              Quaternion.Euler(0, 0, angleOffset) * currAimer.AimDirection() * (sinAngle * Range * 0.5f);

        transform.position = newPosition;
    }
}
