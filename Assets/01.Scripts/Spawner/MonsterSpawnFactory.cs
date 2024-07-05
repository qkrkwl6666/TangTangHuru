using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MonsterSpawnFactory : MonoBehaviour
{
    private Transform playerTransform;

    public float distance = 10f;
     
    private void Awake()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            //CreateMonster(MonsterType.MonsterType1);
        }
        if (Input.GetKeyUp(KeyCode.F2))
        {
            //CreateMonster(MonsterType.MonsterType2);
        }
        if (Input.GetKeyUp(KeyCode.F3))
        {
            //CreateMonster(MonsterType.MonsterType3);
        }
    }

    public void CreateMonster(MonsterType monsterType, int spawnCount, int spawnType)
    {
        switch(spawnType)
        {
            // 沓棋 持失
            case 1:
                for(int i = 0; i < spawnCount; i++)
                {
                    Addressables.InstantiateAsync(monsterType.ToString(), RandomPosition(), Quaternion.identity);
                }
                break;
            // 送識 持失
            case 2:
                for (int i = 0; i < spawnCount; i++)
                {
                    //Addressables.InstantiateAsync(monsterType.ToString(), RandomPosition(), Quaternion.identity);
                }
                break;
            case 3:
                for (int i = 0; i < spawnCount; i++)
                {
                    //Addressables.InstantiateAsync(monsterType.ToString(), RandomPosition(), Quaternion.identity);
                }
                break;
        }
    }
    //public void 

    public Vector2 RandomPosition()
    {
        if (playerTransform == null) return Vector2.zero;

        Vector2 randomCirclePos = (Random.insideUnitCircle * distance);
        Vector2 spawnPos = playerTransform.position * randomCirclePos;

        return spawnPos;
    }


}
