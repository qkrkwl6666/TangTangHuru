using UnityEngine;

public class ManualAim : MonoBehaviour, IAimer
{
    PlayerController controller;
    private GameObject player;
    public GameObject Player { get => player; }
    public float LifeTime { get; set; }
    public float Speed { get; set; }
    public int TotalCount { get; set; }
    public int Index { get; set; }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<PlayerController>();
    }

    public Vector3 AimDirection()
    {
        Vector3 aimPosition = Vector3.zero;

        if (controller.joystick.InputValue == Vector2.zero)
        {
            aimPosition = (Vector3)controller.joystick.prevVector;
        }
        else
        {
            aimPosition = (Vector3)controller.joystick.InputValue;

        }
        return aimPosition.normalized;
    }
}
