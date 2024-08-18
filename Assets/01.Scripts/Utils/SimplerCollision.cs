using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplerCollision : MonoBehaviour
{
    public float minDistance = 1.0f; // 감지 최소 거리
    public float pushForce = 0.1f;   // 밀어내는 힘
    public LayerMask collisionMask;  // 감지할 레이어

    void FixedUpdate() 
    {
        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, minDistance, collisionMask);

        foreach (Collider2D other in nearbyObjects)
        {
            if (other.transform == transform) continue;

            Vector2 direction = (transform.position - other.transform.position).normalized;

            // 오브젝트 서로 밀어냄
            transform.position += (Vector3)direction * pushForce * Time.deltaTime;
            other.transform.position -= (Vector3)direction * pushForce * Time.deltaTime;
        }
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, minDistance);
    //}
}
