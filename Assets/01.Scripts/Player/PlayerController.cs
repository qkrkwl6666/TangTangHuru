
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public JoystickUI joystick;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveInputX = Input.GetAxis("Horizontal");
        float moveInputY = Input.GetAxis("Vertical");

        Vector2 moveVelocity = new Vector2(moveInputX * moveSpeed, moveInputY * moveSpeed);
        //rb.velocity = moveVelocity;

        rb.velocity = joystick.InputValue * moveSpeed;
    }
}
