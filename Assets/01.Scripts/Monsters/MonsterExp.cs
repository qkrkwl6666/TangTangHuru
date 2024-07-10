using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;

public class MonsterExp : MonoBehaviour , IPlayerObserver
{
    private PlayerSubject playerSubject;
    private Transform playerTransform;

    private float exp = 0; //생성시 자동세팅

    private float playerDistanceDifference = 3f;

    private bool isTracking = false;
    private float speed = 20f;

    public IObjectPool<GameObject> pool;

    private void Update()
    {
        if (isTracking)
        {
            Vector2 dir = playerTransform.position - transform.position;
            transform.Translate(dir * speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, playerTransform.position) <= 1)
            {
                // 플레이어 레벨업 메서드 호출
                playerSubject.GetPlayerExp.EarnExp(exp);

                pool.Release(gameObject);
                return;
            } 
        }
    }

    private void OnDisable()
    {
        isTracking = false;
    }

    private void FixedUpdate()
    {
        if (playerSubject == null) return;

        if (Vector2.Distance(transform.position, playerTransform.position) <= playerDistanceDifference)
        {
            isTracking = true;
        }
    }

    public void Initialize(PlayerSubject playerSubject, Transform monsterTransform, float exp)
    {
        if (playerSubject == null)
        {
            Debug.Log("MonsterExp Script PlayerSubject is Null");
            return;
        }
        this.playerSubject = playerSubject;

        playerSubject.AddObserver(this);

        transform.position = monsterTransform.position;
        this.exp = exp;
    }

    public void IObserverUpdate()
    {
        playerTransform = playerSubject.GetPlayerTransform;
    }

    public void Reset()
    {
        
    }
}
