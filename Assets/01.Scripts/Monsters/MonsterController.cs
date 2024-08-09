using UnityEngine;

public class MonsterController : MonoBehaviour, IPlayerObserver
{
    private PlayerSubject playerSubject;

    private Vector2 playerDirection;

    public MonsterView MonsterView { get; private set; }
    public Transform PlayerTransform { get; private set; }

    private float currSpeed = 2f;
    public float MoveSpeed { get; set; } = 2f;
    private bool slowed = false;
    private float slowTimer = 0;
    private float slowTime = 0;

    public Monster Monster { get; private set; }

    public MonsterMoveType MoveType { get; private set; } = MonsterMoveType.Chase;

    public MonsterStateMachine MonsterStateMachine { get; private set; }

    private void Awake()
    {
        playerSubject = GameObject.FindWithTag("PlayerSubject")
            .GetComponent<PlayerSubject>();

        playerSubject.AddObserver(this);

        MonsterView = GetComponentInChildren<MonsterView>();
        Monster = GetComponent<Monster>();

        MonsterStateMachine = new MonsterStateMachine(this, MoveType);
    }

    public void Initialize()
    {

    }

    private void Start()
    {
        MonsterStateMachine.Initialize(MonsterStateMachine.walkState);
    }

    private void Update()
    {


        MonsterStateMachine.Update(Time.deltaTime);
        if (slowed)
        {
            slowTimer += Time.deltaTime;
            if (slowTimer > slowTime)
            {
                currSpeed = MoveSpeed;
                slowed = false;
                slowTimer = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 dir = (PlayerTransform.position - gameObject.transform.position).normalized;

        MonsterView.skeletonRenderer.skeleton.ScaleX = dir.x < 0 ? -1f : 1f;
    }

    private void OnEnable()
    {
        // Todo : 여기서 자신의 위치 타이밍? 이 안맞을 수도 있음 
        playerDirection = (PlayerTransform.position - transform.position).normalized;
        Monster.OnImpact += NuckBack;
    }

    private void OnDestroy()
    {
        Monster.OnImpact -= NuckBack;
    }

    // Todo : currMove 가slowTimer > slowTime 일때만 MoveSpeed 에서 가져와서
    // MoveSpeed 가 적용안되는 문제가 있습니다. 그래서 임시로 일단 MoveSpeed 넣었습니다.
    public void ChasePlayer(float deltaTime)
    {
        if (PlayerTransform == null) return;

        Vector2 dir = (PlayerTransform.position - transform.position).normalized;

        transform.Translate(dir * deltaTime * MoveSpeed);
    }

    public void MoveToInitialPlayerPosition(float deltaTime)
    {
        transform.Translate(playerDirection * deltaTime * MoveSpeed);
    }

    public void IObserverUpdate()
    {
        PlayerTransform = playerSubject.GetPlayerTransform;
    }

    public void Slow(float maxTime)
    {
        slowTimer = 0;
        slowTime = maxTime;
        currSpeed = MoveSpeed * 0.5f;
        slowed = true;
    }
    public void Stop(float maxTime)
    {
        slowTimer = 0;
        slowTime = maxTime;
        currSpeed = 0f;
        slowed = true;
    }

    private void NuckBack(float impact)
    {
        transform.Translate((gameObject.transform.position - PlayerTransform.position).normalized * impact);
    }

}
