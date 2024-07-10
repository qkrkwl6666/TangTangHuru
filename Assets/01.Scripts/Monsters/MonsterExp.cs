using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MonsterExp : MonoBehaviour , IPlayerObserver
{
    private PlayerSubject playerSubject;
    private Transform playerTransform;

    private float exp = 0;

    private float playerDistanceDifference = 3f;

    private bool isTracking = false;
    private float speed = 20f;

    private void Update()
    {
        if (isTracking)
        {
            Vector2 dir = playerTransform.position - transform.position;
            transform.Translate(dir * speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, playerTransform.position) <= 1)
            {
                // Todo : 오브젝트 풀 해야함
                Destroy(gameObject);
            }

            return;
        }
    }

    private void FixedUpdate()
    {
        if (playerSubject == null) return;

        if (Vector2.Distance(transform.position, playerTransform.position) <= playerDistanceDifference)
        {
            // 플레이어 레벨업 메서드 호출
            isTracking = true;
        }
    }

    public void Initialize(PlayerSubject playerSubject, Transform monsterTransform, float exp)
    {
        this.playerSubject = playerSubject;

        if (playerSubject == null)
        {
            Debug.Log("MonsterExp Script PlayerSubject is Null");
            return;
        }

        playerSubject.AddObserver(this);

        transform.position = monsterTransform.position;
        this.exp = exp;
    }

    public void IObserverUpdate()
    {
        playerTransform = playerSubject.GetPlayerTransform;
    }

    public void SetExp(float exp)
    {
        this.exp = exp;
    }
}
