using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SpawnManager : MonoBehaviour
{
    private Transform playerTransform;

    private float distance;

    private void Awake()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    public void CreateMonster(MonsterType monsterType)
    {
        switch (monsterType) 
        {
            case MonsterType.MonsterType1:
                Addressables.InstantiateAsync("");
                break;
            case MonsterType.MonsterType2:

                break;
            case MonsterType.MonsterType3:

                break;
        }
    }

    public Vector2 RandomPosition()
    {
        if (playerTransform == null) return Vector2.zero;

        Vector2 randomCirclePos = (Random.insideUnitCircle * distance);
        Vector2 spawnPos = playerTransform.position * randomCirclePos;

        return spawnPos;
    }




}
