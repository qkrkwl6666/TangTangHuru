using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacknForwardRound : MonoBehaviour, IProjectile
{
    public IAimer currAimer;

    private float timer = 0f;

    public float Range { get; set; }
    public float Size { get; set; }
    public float Speed { get; set; }

    public float ForwardTime = 1.5f;     // ���� �ð�

    private Transform parentTransform;
    private bool movingForward = true; // ���� ���� ������ ���� ������
    private Vector3 startPosition;
    private Vector3 endPosition;

    private float angleOffset = 30f; // Ÿ���� ������ ���� (������Ʈ ���� ���� ���� ��������)

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
        if (timer >= ForwardTime)
        {
            //movingForward = !movingForward;
            timer = 0f;
            UpdatePositions();
        }
        float lerpTime = timer / ForwardTime;
        if (!movingForward)
        {
            lerpTime = 1f - lerpTime;
        }
        
        float angle = lerpTime * 2 * Mathf.PI;
        float cosAngle = Mathf.Cos(angle);
        float sinAngle = Mathf.Sin(angle);

        Vector3 newPosition = parentTransform.position + currAimer.AimDirection() * (cosAngle * Range) + 
                              Quaternion.Euler(0, 0, angleOffset) * currAimer.AimDirection() * (sinAngle * Range * 0.5f);
        
        transform.position = newPosition;
    }

    private void UpdatePositions()
    {
        startPosition = transform.position;
        endPosition = -currAimer.AimDirection() * Range;
    }
}
