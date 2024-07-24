using UnityEngine;

public class Fixed : MonoBehaviour, IProjectile
{
    float timer = 0f;

    IAimer currAimer;
    Vector3 dir;
    Vector3 prevDir = Vector3.zero;

    public float Range { get; set; }
    public float Size { get; set; }
    public float Speed { get; set; }

    void Awake()
    {
        currAimer = GetComponent<IAimer>();
        prevDir.x = 1f;
    }

    private void OnEnable()
    {
        if (currAimer == null)
            return;

        transform.localScale = new Vector3(Size, Size);
        dir = currAimer.AimDirection();

        if (dir == Vector3.zero)
        {
            dir = prevDir;
        }
        else
        {
            prevDir = dir;
        }
        dir *= (Range);

        transform.position = currAimer.Player.transform.position + dir;
        transform.up = dir;
    }

    private void OnDisable()
    {
        timer = 0f;
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
