using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ConstantChaseMove : MonoBehaviour, IPlayerObserver
{
    private Monster monster;

    private Transform playerTransform;
    private PlayerSubject playerSubject;

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
    }

    private void FixedUpdate()
    {
        if (playerTransform == null) return;

        Vector2 dir = (playerTransform.position - gameObject.transform.position).normalized;
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
    }


}
