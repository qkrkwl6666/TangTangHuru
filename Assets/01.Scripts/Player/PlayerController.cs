using UnityEngine;

public enum PlayerState
{
    Idle,
    Run
}

public class PlayerController : MonoBehaviour
{
    public PlayerState state;

    public float moveSpeed = 5f;
    public JoystickUI joystick;

    private SpriteRenderer spriteRenderer;

    public Vector2 velocity;

    Vector3 Left = new Vector3 (-1, 1, 1);
    Vector3 Right = new Vector3 (1, 1, 1);

    public GameObject viewPlayer;

    void Start()
    {
        joystick = GameObject.FindWithTag("GameController").GetComponent<JoystickUI>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        velocity = joystick.InputValue * moveSpeed * Time.deltaTime;

        transform.Translate(velocity);
        
        state = (velocity == Vector2.zero) ? PlayerState.Idle : PlayerState.Run;

        if (joystick.InputValue.x < 0)
        {
            viewPlayer.transform.localScale = Left;
        }
        else if (joystick.InputValue.x > 0)
        {
            viewPlayer.transform.localScale = Right;
        }
    }
}
