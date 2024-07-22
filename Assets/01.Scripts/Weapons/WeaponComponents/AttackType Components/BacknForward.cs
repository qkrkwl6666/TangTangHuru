using UnityEngine;

public class BacknForward : MonoBehaviour, IProjectile
{
    public IAimer currAimer;

    private float timer = 0f;
    private Rigidbody2D rb;

    public float Range { get; set; }
    public float Size { get; set; }
    public float Speed { get; set; }

    public float ForwardTime = 0.5f;     // 전진 시간

    private Transform parentTransform;
    private bool movingForward = true; // 현재 전진 중인지 후진 중인지
    private Vector3 startPosition;
    private Vector3 endPosition;

    void Awake()
    {
        currAimer = GetComponent<IAimer>();
        parentTransform = currAimer.Player.transform;
    }

    private void OnEnable()
    {
        transform.localScale = new Vector3(Size, Size);
        timer = 0f;
        movingForward = true;
        UpdatePositions();
    }

    private void OnDisable()
    {
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= ForwardTime)
        {
            movingForward = !movingForward;
            timer = 0f;
        }
        float lerpTime = timer / ForwardTime;
        if (!movingForward)
        {
            lerpTime = 1f - lerpTime;
        }

        float angle = lerpTime * Mathf.PI; // 반타원 경로를 위해 0부터 PI까지 각도 계산
        float xRadius = Range;
        float yRadius = Range * 0.5f; // y축 반지름을 x축 반지름의 절반으로 설정하여 약간 기울어진 타원 생성

        Vector3 offset = new Vector3(Mathf.Cos(angle) * xRadius, Mathf.Sin(angle) * yRadius, 0);
        Vector3 direction = currAimer.AimDirection();

        // 오브젝트의 전진 방향 기준으로 타원형 경로 계산
        Vector3 rotatedOffset = Quaternion.LookRotation(Vector3.forward, direction) * offset;

        // parentTransform.position을 중심으로 이동 경로 설정
        transform.position = parentTransform.position + rotatedOffset;
    }

    private void UpdatePositions()
    {
        startPosition = parentTransform.position + currAimer.AimDirection() * Range;
        endPosition = parentTransform.position - currAimer.AimDirection() * Range;
    }
}