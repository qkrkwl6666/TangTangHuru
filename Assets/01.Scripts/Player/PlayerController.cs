
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public JoystickUI joystick;

    private Rigidbody2D rb;

    public Vector2 velocity;

    Vector3 Left = new Vector3 (-1, 1, 1);
    Vector3 Right = new Vector3 (1, 1, 1);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        joystick = GameObject.FindWithTag("GameController").GetComponent<JoystickUI>();
    }

    void FixedUpdate()
    {
        velocity = joystick.InputValue * moveSpeed * Time.deltaTime;
        //rb.velocity = velocity;

        transform.Translate(velocity);

        if (joystick.InputValue.x < 0)
        {
            transform.localScale = Left;
        }
        else
        {
            transform.localScale = Right;
        }
    }
}
