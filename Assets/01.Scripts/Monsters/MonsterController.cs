using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour, IPlayerObserver
{
    private PlayerSubject playerSubject;
    
    private Vector2 playerDirection;

    public MonsterView MonsterView { get; private set; }
    public Transform PlayerTransform { get; private set; }

    private float moveSpeed = 3f;
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
        MonsterStateMachine.Update(Time.deltaTime);
    }

    private void OnEnable()
    {
        // Todo : 여기서 자신의 위치 타이밍? 이 안맞을 수도 있음 
        playerDirection = (PlayerTransform.position - transform.position).normalized;
    }

    public void ChasePlayer(float deltaTime)
    {
        if (PlayerTransform == null) return;

        Vector2 dir = (PlayerTransform.position - transform.position).normalized;

        transform.Translate(dir * deltaTime * moveSpeed);
    }

    public void MoveToInitialPlayerPosition(float deltaTime)
    {
        transform.Translate(playerDirection * deltaTime * moveSpeed);
    }

    public void IObserverUpdate()
    {
        PlayerTransform = playerSubject.GetPlayerTransform;
    }

}
