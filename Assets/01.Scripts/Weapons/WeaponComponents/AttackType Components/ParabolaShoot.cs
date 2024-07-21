using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaShoot : MonoBehaviour, IProjectile
{
    public IAimer currAimer;

    private float timer = 0f;
    private Rigidbody2D rb;

    private Vector2 initialPosition;
    private Vector2 targetPosition;

    public float Range { get; set; }
    public float Size { get; set; }
    public float Speed { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currAimer = GetComponent<IAimer>();
    }

    private void OnEnable()
    {
        timer = 0f;
        transform.localScale = new Vector3(Size, Size);

        initialPosition = transform.position;
        Vector2 aimDirection = currAimer.AimDirection();
        targetPosition = initialPosition + aimDirection * currAimer.LifeTime * Speed;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        float progress = timer / currAimer.LifeTime;

        if (progress >= 1f)
        {
            transform.position = targetPosition;
            gameObject.SetActive(false);
            return;
        }

        Vector2 linearPosition = Vector2.Lerp(initialPosition, targetPosition, progress);
        float heightOffset = Mathf.Sin(Mathf.PI * progress) * Speed / 2;

        Vector2 parabolaPosition = linearPosition + Vector2.up * heightOffset;
        transform.position = parabolaPosition;
    }
}
