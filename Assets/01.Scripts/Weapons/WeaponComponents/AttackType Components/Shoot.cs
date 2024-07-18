using UnityEngine;

public class Shoot : MonoBehaviour
{
    public IAimer currAimer;

    private float timer = 0f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currAimer = GetComponent<IAimer>();
    }

    private void OnEnable()
    {
        timer = 0f;
        var pos = currAimer.AimDirection();
        transform.up = pos;
        rb.velocity = pos * currAimer.Speed;
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

