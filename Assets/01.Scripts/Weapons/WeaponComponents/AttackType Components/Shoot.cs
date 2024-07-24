using UnityEngine;

public class Shoot : MonoBehaviour, IProjectile
{
    public IAimer currAimer;

    private float timer = 0f;
    private Vector3 dir = Vector3.zero;

    public float Range { get; set; }
    public float Size { get; set; }
    public float Speed { get; set; }

    private void Awake()
    {
        currAimer = GetComponent<IAimer>();
    }

    private void OnEnable()
    {
        timer = 0f;
        transform.localScale = new Vector3(Size, Size);

        dir = currAimer.AimDirection();
        transform.up = dir;
    }


    private void Update()
    {
        if (timer > currAimer.LifeTime)
        {
            gameObject.SetActive(false);
        }
        else
        {
            transform.Translate(dir * Speed * Time.deltaTime, Space.World);
            timer += Time.deltaTime;
        }
    }
}

