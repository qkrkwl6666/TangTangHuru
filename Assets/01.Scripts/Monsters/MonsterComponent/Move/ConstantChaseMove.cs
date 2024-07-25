using UnityEngine;

public class ConstantChaseMove : MonoBehaviour, IPlayerObserver
{
    private Monster monster;

    private Transform playerTransform;
    private PlayerSubject playerSubject;

    private SpriteRenderer spriteRenderer;

    public void Initialize(PlayerSubject playerSubject)
    {
        this.playerSubject = playerSubject;

        if (playerSubject == null)
        {
            Debug.Log("ConstantChaseMove Script PlayerSubject is Null");
            return;
        }

        playerSubject.AddObserver(this);
    }

    private void Awake()
    {
        monster = GetComponent<Monster>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        monster.OnDamaged += NuckBack;
    }

    private void FixedUpdate()
    {
        if (playerTransform == null) return;

        Vector2 dir = (playerTransform.position - gameObject.transform.position).normalized;

        spriteRenderer.flipX = dir.x < 0 ? true : false;

        transform.Translate(dir * Time.deltaTime * monster.MoveSpeed);
    }

    public void IObserverUpdate()
    {
        playerTransform = playerSubject.GetPlayerTransform;
    }

    private void OnDestroy()
    {
        if (playerTransform == null) return;

        playerSubject.RemoveObserver(this);
        monster.OnDamaged -= NuckBack;
    }

    private void NuckBack(float damage)
    {
        transform.Translate((gameObject.transform.position - playerTransform.position).normalized * 0.3f);
    }
}
