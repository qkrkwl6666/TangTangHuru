using System.Threading;
using UnityEngine;

public class MonsterController : MonoBehaviour, IPlayerObserver
{
    private PlayerSubject playerSubject;
    
    private Vector2 playerDirection;

    public MonsterView MonsterView { get; private set; }
    public Transform PlayerTransform { get; private set; }

    private float currSpeed = 2f;
    private float moveSpeed = 2f;
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

        MonsterStateMachine = new MonsterStateMachine(this, MoveType);

        Monster = GetComponent<Monster>();
    }

    private void Start()
    {
        MonsterStateMachine.Initialize(MonsterStateMachine.walkState);
    }

    private void Update()
    {
        Vector2 dir = (PlayerTransform.position - gameObject.transform.position).normalized;


        MonsterView.skeletonAnimation.skeleton.ScaleX = dir.x < 0 ? -1f : 1f;

        MonsterStateMachine.Update(Time.deltaTime);

        if (slowed)
        { 
            slowTimer += Time.deltaTime;
            if(slowTimer > slowTime)
            {
                currSpeed = moveSpeed;
                slowed = false;
                slowTimer = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        
    }

    private void OnEnable()
    {
        // Todo : 여기서 자신의 위치 타이밍? 이 안맞을 수도 있음 
        playerDirection = (PlayerTransform.position - transform.position).normalized;
    }

    private void OnDestroy()
    {
        Monster.OnImpact -= NuckBack;
    }

    public void ChasePlayer(float deltaTime)
    {
        if (PlayerTransform == null) return;

        Vector2 dir = (PlayerTransform.position - transform.position).normalized;

        transform.Translate(dir * deltaTime * currSpeed);
    }

    public void MoveToInitialPlayerPosition(float deltaTime)
    {
        transform.Translate(playerDirection * deltaTime * currSpeed);
    }

    public void IObserverUpdate()
    {
        PlayerTransform = playerSubject.GetPlayerTransform;
    }

    public void Slow(float maxTime)
    {
        slowTimer = 0;
        slowTime = maxTime;
        currSpeed = moveSpeed * 0.5f;
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
