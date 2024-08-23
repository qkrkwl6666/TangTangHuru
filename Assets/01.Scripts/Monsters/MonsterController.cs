using DG.Tweening;
using UnityEngine;

public class MonsterController : MonoBehaviour, IPlayerObserver
{
    private PlayerSubject playerSubject;

    private Vector2 playerDirection;

    public MonsterView MonsterView { get; private set; }
    public Transform PlayerTransform { get; private set; }

    public float MoveSpeed { get; set; } = 2f;
    private bool slowed = false;
    private float slowTimer = 1.5f;
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
            slowTime += Time.deltaTime;
            if (slowTime > slowTimer)
            {
                slowed = false;
                slowTime = 0;
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

    public void ChasePlayer(float deltaTime)
    {
        if (PlayerTransform == null) return;

        Vector2 dir = (PlayerTransform.position - transform.position).normalized;

        if (!slowed)
        {
            transform.Translate(dir * deltaTime * MoveSpeed);
        }
        else
        {
            transform.Translate(dir * deltaTime * (MoveSpeed * 0.4f));
        }
    }

    public void MoveToInitialPlayerPosition(float deltaTime)
    {
        if (!slowed)
        {
            transform.Translate(playerDirection * deltaTime * MoveSpeed);
        }
        else
        {
            transform.Translate(playerDirection * deltaTime * (MoveSpeed * 0.4f));
        }
    }

    public void IObserverUpdate()
    {
        PlayerTransform = playerSubject.GetPlayerTransform;
    }

    public void Slow()
    {
        slowTime = 0;
        slowed = true;
    }

    private void NuckBack(float impact)
    {
        //transform.Translate((gameObject.transform.position - PlayerTransform.position).normalized * impact);

        if (Monster.dead)
        {
            Vector3 direction = (transform.position - PlayerTransform.position).normalized;
            Vector3 targetPosition = transform.position + direction * impact;

            transform.DOMove(targetPosition, 0.3f).SetEase(Ease.OutQuad);
        }


    }

}
