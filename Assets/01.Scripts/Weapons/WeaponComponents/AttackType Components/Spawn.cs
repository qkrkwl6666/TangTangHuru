using UnityEngine;

public class Spawn : MonoBehaviour, IProjectile
{

    float timer = 0f;

    IAimer currAimer;
    Vector3 pos;
    public float Range { get; set; }
    public float Size { get; set; }
    public float Speed { get; set; }

    void Awake()
    {
        currAimer = GetComponent<IAimer>();
    }

    private void OnEnable()
    {
        if (currAimer == null)
            return;

        pos = currAimer.AimDirection();

        transform.position = pos;
        transform.localScale = new Vector3(Size, Size);
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
