using System.Collections;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Run,
    Stun,
}

public class PlayerController : MonoBehaviour
{
    public PlayerState state;

    public float moveSpeed = 5f;
    public JoystickUI joystick;

    private SpriteRenderer spriteRenderer;

    public Vector2 velocity;

    Vector3 Left = new Vector3(-1, 1, 1);
    Vector3 Right = new Vector3(1, 1, 1);

    public GameObject viewPlayer;

    private bool isStun = false;

    void Start()
    {
        joystick = GameObject.FindWithTag("GameController").GetComponent<JoystickUI>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    void FixedUpdate()
    {
        if (isStun) return;

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

    public void StartStun(float stunDuration)
    {
        StartCoroutine(Stunned(stunDuration));
    }

    public IEnumerator Stunned(float stunDuration)
    {
        isStun = true;
        state = PlayerState.Stun;

        yield return new WaitForSeconds(stunDuration);

        isStun = false;
        state = PlayerState.Idle;
    }

}
