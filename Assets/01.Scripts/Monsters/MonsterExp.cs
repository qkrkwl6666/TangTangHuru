using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MonsterExp : MonoBehaviour , IPlayerObserver
{
    private PlayerSubject playerSubject;
    private Transform playerTransform;

    private float playerDistanceDifference = 3f;
    private float duration = 0.5f;
    private float time = 0f;

    public bool isTracking = false;
    public float speed = 3f;

    private void Update()
    {
        if (playerSubject == null) return;

        time += Time.deltaTime;

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

        if (time >= duration)
        {
            time = 0f;
            if(Vector2.Distance(transform.position, playerTransform.position) <= playerDistanceDifference)
            {
                // 플레이어 레벨업 메서드 호출
                isTracking = true;
            }
        }
    }

    public void Initialize(PlayerSubject playerSubject, Transform monsterTransform)
    {
        this.playerSubject = playerSubject;

        if (playerSubject == null)
        {
            Debug.Log("MonsterExp Script PlayerSubject is Null");
            return;
        }

        playerSubject.AddObserver(this);
    }

    public void IObserverUpdate()
    {
        playerTransform = playerSubject.GetPlayerTransform;
    }
}
