using UnityEngine;

public class PlayerAim : MonoBehaviour, IAimer
{
    public GameObject player;
    public GameObject Player { get => player; }
    public float LifeTime { get; set; }
    public float Speed { get; set; }
    public int TotalCount { get; set; }
    public int Index { get; set; }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public Vector3 AimDirection()
    {
        return Player.transform.position;
    }
}
