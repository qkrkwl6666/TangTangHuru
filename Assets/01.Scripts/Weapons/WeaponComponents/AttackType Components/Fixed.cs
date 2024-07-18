using UnityEngine;

public class Fixed : MonoBehaviour
{
    public float range = 2f;

    float timer = 0f;

    IAimer currAimer;
    Vector3 dir;
    Vector3 prevDir = Vector3.zero;

    void Awake()
    {
        currAimer = GetComponent<IAimer>();
        prevDir.x = 1f;
    }

    private void OnEnable()
    {
        if (currAimer == null)
            return;

        dir = currAimer.AimDirection();

        if (dir == Vector3.zero)
        {
            dir = prevDir;
        }
        else
        {
            prevDir = dir;
        }
        dir *= (range + 1);

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
