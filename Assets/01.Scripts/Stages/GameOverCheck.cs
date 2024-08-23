using UnityEngine;

public class GameOverCheck : MonoBehaviour, IPlayerObserver
{
    public TimeControl timeControl;
    public GameObject gameOverMenu;

    private PlayerSubject playerSubject;
    private PlayerHealth playerHp;
    public void IObserverUpdate()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(gameObject.name);

        playerSubject = GameObject.FindWithTag("PlayerSubject").GetComponent<PlayerSubject>();
        playerSubject.AddObserver(this);

        playerHp = playerSubject.GetPlayerLivingEntity as PlayerHealth;
        playerHp.onDeath += GameOver;
    }

    private void OnDestroy()
    {
        playerHp.onDeath -= GameOver;
    }

    private void GameOver()
    {
        timeControl.StopTime();
        gameOverMenu.SetActive(true);
    }
}
