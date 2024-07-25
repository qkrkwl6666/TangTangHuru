using UnityEngine;

public class SkillUpgradeManager : MonoBehaviour, IPlayerObserver
{
    public GameObject levelUpMenu;

    private PlayerSubject playerSubject;
    private PlayerExp playerExp;


    void Start()
    {
        playerSubject = GameObject.FindWithTag("PlayerSubject").GetComponent<PlayerSubject>();
        playerSubject.AddObserver(this);

        playerExp = playerSubject.GetPlayerExp;
        playerExp.OnLevelChanged += HandleLevelChanged;
    }

    private void OnDisable()
    {
        playerExp.OnLevelChanged -= HandleLevelChanged;
    }

    private void HandleLevelChanged(int newLevel)
    {
        //Debug.Log("Player level changed to: " + newLevel);
        // ���� ���濡 ���� ���� ó��
        levelUpMenu.SetActive(true);
    }

    public void IObserverUpdate()
    {

    }
}
