using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUpgradeManager : MonoBehaviour, IPlayerObserver
{
    private PlayerSubject playerSubject;

    void Start()
    {
        playerSubject = GameObject.FindWithTag("PlayerSubject").GetComponent<PlayerSubject>();
        playerSubject.AddObserver(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void IObserverUpdate()
    {

    }
}
