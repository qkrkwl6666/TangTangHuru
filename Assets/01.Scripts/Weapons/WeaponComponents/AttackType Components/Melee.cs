using UnityEngine;

public class Melee : MonoBehaviour
{
    public float range = 2f;

    float timer = 0f;

    IAimer currAimer;
    Vector3 dir;

    void Awake()
    {
        currAimer = GetComponent<IAimer>();
    }
    private void OnEnable()
    {
        if (currAimer == null)
            return;

        dir = currAimer.AimDirection();

        if(currAimer.AimDirection() == currAimer.Player.transform.position)
        {
            dir = Vector3.zero; //������ �÷��̾� ��ġ�� dir 0���� ����. �߽ɿ��� ����ٴϴ� ��ų
        }
        else
        {
            dir = dir.normalized;
        }

        dir *= range;
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
            transform.position = currAimer.Player.transform.position + dir;
            transform.up = dir;
            timer += Time.deltaTime;
        }
    }
}
