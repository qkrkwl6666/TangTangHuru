using UnityEngine;

public class AngularAim : MonoBehaviour, IAimer
{
    GameObject player;
    public GameObject Player { get => player; }

    public float LifeTime { get; set; }
    public float Speed { get; set; }
    public int TotalCount { get; set; }
    public int Index { get; set; }

    private Vector2 direction;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public Vector3 AimDirection()
    {
        float angle = 0f;
        if (TotalCount == 2)
        {
            angle = (180f / TotalCount) * Index;

        }
        else if(TotalCount == 3)
        {
            angle = (360f / TotalCount) * Index;
            angle -= 90;
        }
        else
        {
            angle = (360f / TotalCount) * Index;
        }
        angle *= Mathf.Deg2Rad;
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        return direction;
    }
}
