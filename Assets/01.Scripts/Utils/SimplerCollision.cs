using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplerCollision : MonoBehaviour
{
    public float minDistance = 1.0f; // ���� �ּ� �Ÿ�
    public float pushForce = 0.1f;   // �о�� ��
    public LayerMask collisionMask;  // ������ ���̾�

    void FixedUpdate() 
    {
        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, minDistance, collisionMask);

        foreach (Collider2D other in nearbyObjects)
        {
            if (other.transform == transform) continue;

            Vector2 direction = (transform.position - other.transform.position).normalized;

            // ������Ʈ ���� �о
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
