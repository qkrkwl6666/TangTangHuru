using UnityEngine;

public class Heal : MonoBehaviour, IInGameItem, IPlayerObserver
{
    public int ItemId { get; set; }
    public string Name { get; set; }
    public IItemType ItemType { get; set; }
    public string TextureId { get; set; }

    private PlayerSubject playerSubject;

    private PlayerHealth playerHealth;

    private float health = 50f;

    private void Awake()
    {
        playerSubject = GameObject.FindWithTag("PlayerSubject").GetComponent<PlayerSubject>();

        playerSubject.AddObserver(this);
    }

    public void GetItem()
    {

    }

    public void UseItem()
    {
        playerHealth.Health(health);
        gameObject.SetActive(false);
    }

    public void IObserverUpdate()
    {
        playerHealth = playerSubject.GetPlayerLivingEntity as PlayerHealth;
    }
}
