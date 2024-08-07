using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float updateInterval = 0.5f;
    public float detectionRadius = 10f;

    private Transform playerTransform;
    private Vector3 targetPosition;
    private float timer;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player with tag 'Player' not found.");
        }

        SetRandomTargetPosition();
    }

    void Update()
    {
        if (playerTransform != null)
        {
            MoveTowardsTarget();
            HandleTargetUpdate();
        }
    }

    void MoveTowardsTarget()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void HandleTargetUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            SetRandomTargetPosition();
            timer = 0f;
        }
    }

    void SetRandomTargetPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * detectionRadius;
        randomDirection.z = 0;
        targetPosition = playerTransform.position + randomDirection;
    }
}
