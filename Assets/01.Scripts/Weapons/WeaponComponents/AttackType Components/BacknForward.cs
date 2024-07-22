using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BacknForward : MonoBehaviour, IProjectile
{
    public IAimer currAimer;

    private float timer = 0f;
    private Rigidbody2D rb;

    public float Range { get; set; }
    public float Size { get; set; }
    public float Speed { get; set; }

    public float ForwardTime = 0.7f;     // 전진 시간

    private Transform parentTransform;
    private bool movingForward = true; // 현재 전진 중인지 후진 중인지
    private Vector3 startPosition;
    private Vector3 endPosition;

    void Awake()
    {
        currAimer = GetComponent<IAimer>();
        parentTransform = currAimer.Player.transform;
    }

    private void OnEnable()
    {
        transform.localScale = new Vector3(Size, Size);
        timer = 0f;
        movingForward = true;
        UpdatePositions();
    }

    private void OnDisable()
    {
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        float lerpTime = timer / ForwardTime;

        transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.SmoothStep(0f, 1f, lerpTime));


        if (timer >= ForwardTime)
        {
            movingForward = !movingForward;
            timer = 0f;
            UpdatePositions();
        }
    }


    private void UpdatePositions()
    {
        startPosition = transform.position;
        endPosition = movingForward 
            ? parentTransform.position + currAimer.AimDirection() * Speed
            : parentTransform.position - currAimer.AimDirection() * Speed;
    }
}
