using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DirectionArrow : MonoBehaviour
{
    private PlayerController controller;
    public Image triangle;
    public float radius = 2f;

    private Vector2 velocity;

    void Start()
    {
        controller = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        if (controller != null)
        {
            velocity = controller.velocity;
        }
        if (velocity != Vector2.zero)
        {
            Vector2 direction = velocity.normalized;
            Vector2 trianglePosition = (Vector2)transform.position + direction * radius; // �ﰢ���� ��ġ ���

            triangle.transform.position = trianglePosition;

            // �ﰢ���� ȸ�� ���� (�̵� ������ ����Ű����)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            triangle.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
