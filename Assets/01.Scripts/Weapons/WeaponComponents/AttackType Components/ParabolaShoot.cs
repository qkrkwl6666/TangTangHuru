using UnityEngine;

public class ParabolaShoot : MonoBehaviour, IProjectile
{
    public IAimer currAimer;

    private float timer = 0f;
    private Rigidbody2D rb;

    private Vector2 initialPosition;
    private Vector2 targetPosition;

    public float Range { get; set; }
    public float Size { get; set; }
    public float Speed { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currAimer = GetComponent<IAimer>();
    }

    private void OnEnable() //UI�� ��ǥ ����
    {
        timer = 0f;
        transform.localScale = new Vector3(Size, Size);

        initialPosition = transform.position;

        // ȭ�� �߽ɿ��� Ư�� �������� ���� �Ÿ��� �ִ� ��ǥ ���� ����
        Vector2 aimDirection = currAimer.AimDirection().normalized;
        Vector2 screenCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        targetPosition = screenCenter + aimDirection * Range;
    }


    private void Update()
    {
        timer += Time.deltaTime;

        float progress = timer / currAimer.LifeTime;

        if (progress >= 1f)
        {
            transform.position = targetPosition;
            gameObject.SetActive(false);
            return;
        }

        Vector2 linearPosition = Vector2.Lerp(initialPosition, targetPosition, progress);

        // ������ ������ ���� ������ ���
        float heightOffset = Mathf.Sin(Mathf.PI * progress) * Speed / 2;

        Vector2 parabolaPosition = linearPosition + Vector2.up * heightOffset;
        transform.position = parabolaPosition;
    }
}
