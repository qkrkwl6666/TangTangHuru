using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUpgradeManager : MonoBehaviour, IPlayerObserver
{
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
        Debug.Log("Player level changed to: " + newLevel);
        // 레벨 변경에 따른 로직 처리
    }

    public void IObserverUpdate()
    {

    }
}
