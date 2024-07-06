using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSubject : MonoBehaviour
{
    private List<IPlayerObserver> playerObservers = new List<IPlayerObserver>();

    private LivingEntity playerLivingEntity;

    public LivingEntity GetPlayerLivingEntity { get { return playerLivingEntity; } }
    public Transform GetPlayerTransform { get { return playerLivingEntity.transform; } }

    private void Awake()
    {
        playerLivingEntity = GameObject.FindWithTag("Player").GetComponent<LivingEntity>();
    }

    public void NotifyObserver()
    {
        foreach (var observer in playerObservers)
        {
            observer.IObserverUpdate();
        }
    }

    public void AddObserver(IPlayerObserver playerObserver)
    {
        if (playerObservers.Contains(playerObserver)) 
            return;

        playerObservers.Add(playerObserver);

        playerObserver.IObserverUpdate();
    }

    public void RemoveObserver(IPlayerObserver playerObserver)
    {
        playerObservers.Remove(playerObserver);
    }
}
