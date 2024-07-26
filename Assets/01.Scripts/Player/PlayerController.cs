using System.Collections.Generic;
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

    Vector3 Left = new Vector3(-1, 1, 1);
    Vector3 Right = new Vector3(1, 1, 1);

    public GameObject viewPlayer;

    private void Awake()
    {
        switch (GameManager.Instance.currentWeapon)
        {
            case "OneSword":
                weapons[0].SetActive(true);
                break;
            case "Axe":
                weapons[1].SetActive(true);
                break;
            case "Bow":
                weapons[2].SetActive(true);
                break;
            case "Crossbow":
                weapons[3].SetActive(true);
                break;
            case "Wand":
                weapons[4].SetActive(true);
                break;
            case "Staff":
                weapons[5].SetActive(true);
                break;
        }
    }

    void Start()
    {
        joystick = GameObject.FindWithTag("GameController").GetComponent<JoystickUI>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Todo : 임시 코드


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

    // Todo : 임시용 코드
    public List<GameObject> weapons = new List<GameObject>();



}
