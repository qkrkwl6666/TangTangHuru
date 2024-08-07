using UnityEngine;
using UnityEngine.Pool;

public class MonsterExp : MonoBehaviour, IPlayerObserver
{
    private PlayerSubject playerSubject;
    private Transform playerTransform;

    private float exp = 0; //생성시 자동세팅

    private float playerDistanceDifference = 0.5f;

    private float speed = 5f;

    private Transform targetTransform;

    public IObjectPool<GameObject> pool;

    private void Update()
    {
        if (playerSubject == null) return;

        if (Vector2.Distance(transform.position, playerTransform.position) <= playerDistanceDifference)
        {
            targetTransform = playerTransform;
        }

        if (targetTransform == null)
            return;

        Vector2 dir = targetTransform.position - transform.position;
        transform.Translate(dir * speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, targetTransform.position) <= 1)
        {
            Release();
            return;
        }
    }

    public void Release()
    {
        playerSubject.GetPlayerExp.EarnExp(exp);
        pool.Release(gameObject);
    }

    private void OnDisable()
    {
        targetTransform = null;
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

    public void SetTarget(Transform target)
    {
        targetTransform = target;
    }
}
