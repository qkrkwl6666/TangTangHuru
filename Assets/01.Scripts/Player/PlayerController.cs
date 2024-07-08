
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public JoystickUI joystick;

    private Rigidbody2D rb;

    public Vector2 velocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        velocity = joystick.InputValue * moveSpeed;
        rb.velocity = velocity;
    }
}
