using UnityEngine;

public class SkillUpgradeManager : MonoBehaviour, IPlayerObserver
{
    public GameObject levelUpMenu;

    private PlayerSubject playerSubject;
    private PlayerExp playerExp;

    private int levelUpCount = 0;
    void Start()
    {
        playerSubject = GameObject.FindWithTag("PlayerSubject").GetComponent<PlayerSubject>();

        playerSubject.AddObserver(this);

        playerExp = playerSubject.GetPlayerExp;
        playerExp.OnLevelChanged += HandleLevelChanged;

        if(playerSubject == null)
        {
            Debug.Log("플레이어 서브젝트 찾을 수 없음");
        }
    }

    private void OnDisable()
    {
        playerExp.OnLevelChanged -= HandleLevelChanged;
    }

    private void HandleLevelChanged(int newLevel)
    {
        levelUpCount++;

        if (levelUpMenu.activeSelf)
            return;

        if (levelUpCount > 0)
        {
            levelUpMenu.SetActive(true);
            levelUpCount--;
        }
    }
    public void CheckLevelUpPanelOn()
    {
        if(levelUpCount > 0)
        {
            Invoke("ActivateLevelUpMenu", 0.2f);
            levelUpCount--;
        }
    }

    private void ActivateLevelUpMenu()
    {
        levelUpMenu.SetActive(true);
    }

    public void IObserverUpdate()
    {

    }
}
