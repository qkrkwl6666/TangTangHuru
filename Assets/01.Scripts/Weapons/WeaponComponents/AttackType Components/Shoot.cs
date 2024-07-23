using UnityEngine;

public class Shoot : MonoBehaviour, IProjectile
{
    public IAimer currAimer;

    private float timer = 0f;
    private Rigidbody2D rb;

    public float Range { get; set; }
    public float Size { get; set; }
    public float Speed { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currAimer = GetComponent<IAimer>();
    }

    private void OnEnable()
    {
        timer = 0f;
        transform.localScale = new Vector3(Size, Size);

        var pos = currAimer.AimDirection();
        transform.up = pos;
        rb.velocity = pos * Speed;
    }


    private void Update()
    {
        if (timer > currAimer.LifeTime)
        {
            gameObject.SetActive(false);
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}

