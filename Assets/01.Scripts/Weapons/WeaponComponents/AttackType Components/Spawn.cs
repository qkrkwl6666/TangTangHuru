using UnityEngine;

public class Spawn : MonoBehaviour
{
    public float range = 2f;

    float timer = 0f;

    IAimer currAimer;
    Vector3 pos;

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
