using UnityEngine;

public class Melee : MonoBehaviour, IProjectile
{
    float timer = 0f;

    IAimer currAimer;
    Vector3 dir;

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

        dir = currAimer.AimDirection();

        if(currAimer.AimDirection() == currAimer.Player.transform.position)
        {
            dir = Vector3.zero; //������ �÷��̾� ��ġ�� dir 0���� ����. �߽ɿ��� ����ٴϴ� ��ų
        }
        else
        {
            dir = dir.normalized;
        }
        dir *= Range;
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
            transform.position = currAimer.Player.transform.position + dir;
            transform.up = dir;
            timer += Time.deltaTime;
        }
    }
}
