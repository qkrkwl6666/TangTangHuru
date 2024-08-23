using UnityEngine;

public class PetController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float updateInterval = 0.5f;
    public float detectionRadius = 10f;

    private Transform playerTransform;
    private Vector3 targetPosition;
    private float timer;

    private bool isSettled = false;

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

        var difference = Vector3.Distance(transform.position, targetPosition);
        if (difference < 0.3f)
        {
            transform.position = targetPosition;
        }
    }

    void HandleTargetUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            if (!isSettled)
            {
                SetRandomTargetPosition();
                timer = 0f;
            }
        }

        if (timer >= 8f)
        {
            isSettled = false;
            timer = 0f;
        }
    }

    void SetRandomTargetPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * detectionRadius;
        randomDirection.z = 0;
        var difference = Vector3.Distance(playerTransform.position, randomDirection);

        if (difference < 2)
        {
            targetPosition = transform.position;
        }
        else
        {
            targetPosition = playerTransform.position + randomDirection;
        }

        if (targetPosition.x < transform.position.x)
        {
            var scale = gameObject.transform.localScale;
            scale.x = 1;
            gameObject.transform.localScale = scale;
        }
        else
        {
            var scale = gameObject.transform.localScale;
            scale.x = -1;
            gameObject.transform.localScale = scale;
        }
    }

    public void SetTargetPosition(Vector3 postion)
    {
        targetPosition = postion;
        isSettled = true;
    }
}
