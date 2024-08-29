using UnityEngine;
using UnityEngine.Pool;

public class MonsterExp : MonoBehaviour, IPlayerObserver, IInGameItem
{
    private PlayerSubject playerSubject;
    private Transform playerTransform;

    private float exp = 0; //생성시 자동세팅

    private float speed = 10f;

    private Transform targetTransform;

    public IObjectPool<GameObject> pool;

    public int ItemId { get; set; }
    public string Name { get; set; }
    public IItemType ItemType { get; set; }
    public string TextureId { get; set; }

    private bool isUsed = false;

    public void Release()
    {
        if (!isUsed)
        {
            isUsed = true;
            playerSubject.GetPlayerExp.EarnExp(exp);
            pool.Release(gameObject);
        }
    }

    private void OnEnable()
    {
        isUsed = false;
    }

    private void OnDisable()
    {
        targetTransform = null;
    }

    public void Initialize(PlayerSubject playerSubject, Transform monsterTransform, float exp)
    {
        if (playerSubject == null)
        {
            //Debug.Log("MonsterExp Script PlayerSubject is Null");
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

    public void SetTarget(Transform target)
    {
        targetTransform = target;
    }

    public void UseItem()
    {
        if (!isUsed)
        {
            SoundManager.Instance.PlayShortSound("exp");
            Release();
        }
    }

    public void GetItem()
    {

    }

    private void Update()
    {
        if (targetTransform == null)
            return;

        var dir = targetTransform.position - transform.position;

        transform.position += dir.normalized * speed * Time.deltaTime;


        if (Vector2.Distance(targetTransform.position, transform.position) < 0.5f)
        {
            UseItem();
        }
    }
}
