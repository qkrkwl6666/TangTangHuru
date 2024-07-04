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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            CreateMonster(MonsterType.MonsterType1);
        }
        if (Input.GetKeyUp(KeyCode.F2))
        {
            CreateMonster(MonsterType.MonsterType2);
        }
        if (Input.GetKeyUp(KeyCode.F3))
        {
            CreateMonster(MonsterType.MonsterType3);
        }
    }

    public void CreateMonster(MonsterType monsterType)
    {
        switch (monsterType) 
        {
            case MonsterType.MonsterType1:
                Addressables.InstantiateAsync("MonsterType1", RandomPosition(), Quaternion.identity);
                break;
            case MonsterType.MonsterType2:
                Addressables.InstantiateAsync("MonsterType2", RandomPosition(), Quaternion.identity);
                break;
            case MonsterType.MonsterType3:
                Addressables.InstantiateAsync("MonsterType3", RandomPosition(), Quaternion.identity);
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
