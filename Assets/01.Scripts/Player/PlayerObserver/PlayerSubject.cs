using System.Collections.Generic;
using UnityEngine;

public class PlayerSubject : MonoBehaviour
{
    private List<IPlayerObserver> playerObservers = new List<IPlayerObserver>();

    private GameObject playerObject;
    private LivingEntity playerLivingEntity;
    private PlayerExp playerExp;


    public LivingEntity GetPlayerLivingEntity { get { return playerLivingEntity; } }
    public Transform GetPlayerTransform { get { return playerObject.transform; } }
    public PlayerExp GetPlayerExp { get { return playerExp; } }


    private void Awake()
    {
        playerObject = GameObject.FindWithTag("Player");
        playerLivingEntity = playerObject.GetComponent<LivingEntity>();
        playerExp = playerObject.GetComponent<PlayerExp>();
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
