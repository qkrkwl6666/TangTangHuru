using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AdjustMonsterPosition : MonoBehaviour, IPlayerObserver
{
    private PlayerSubject playerSubject;
    private Transform playerTransfrom;

    private float playerDistanceDifference = 25f;
    private float defaultDistance = 20f;
    private float duration = 1f;
    private float time = 0f;

    public void Update()
    {
        if (playerTransfrom == null) return;

        time += Time.deltaTime;

        if (time >= duration)
        {
            time = 0f;
            if(Vector2.Distance(transform.position, (Vector2)playerTransfrom.position) >= playerDistanceDifference)
            {
                transform.position = MonsterSpawnFactory.RandomPosition(playerTransfrom, defaultDistance);
            }
        }
    }

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

    public void IObserverUpdate()
    {
        playerTransfrom = playerSubject.GetPlayerTransform;
    }

    private void OnDestroy()
    {
        if (playerSubject == null) return;

        playerSubject.RemoveObserver(this);
    }
}
