using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MonsterSpawnFactory : MonoBehaviour
{
    private Transform playerTransform;

    private float defaultDistance = 10f;
    private float circleSpawnDistance = 10f;
    private float lineSpawnDistance = 5f;
     
    private void Awake()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            //CreateMonster(MonsterType.MonsterType1, 30, 3);
        }
        if (Input.GetKeyUp(KeyCode.F2))
        {
            //CreateMonster(MonsterType.MonsterType2, 10, 2);
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
            // 罚待 积己
            case 1:
                for(int i = 0; i < spawnCount; i++)
                {
                    Addressables.InstantiateAsync(monsterType.ToString(), RandomPosition(defaultDistance), Quaternion.identity);
                }
                break;
            // 流急 积己
            case 2:
                var lineList = LinePosition(RandomPosition(lineSpawnDistance), spawnCount, 0f);

                for (int i = 0; i < spawnCount; i++)
                {
                    Addressables.InstantiateAsync(monsterType.ToString(), lineList[i], Quaternion.identity);
                }
                break;
            // 盔 积己
            case 3:
                for (int i = 0; i < spawnCount; i++)
                {
                    Addressables.InstantiateAsync(monsterType.ToString(), CirclePosition(spawnCount, i), Quaternion.identity);
                }
                break;
        }
    }
    //public void 

    public Vector2 RandomPosition(float distance)
    {
        if (playerTransform == null) return Vector2.zero;

        Vector2 randomCirclePos = (Random.insideUnitCircle * distance);
        Vector2 spawnPos = (Vector2)playerTransform.position + randomCirclePos;

        return spawnPos;
    }

    public Vector2 CirclePosition(int spawnCount, int currentSpawnCount)
    {
        if (playerTransform == null) return Vector2.zero;

        float angle = ((360 / spawnCount) * currentSpawnCount) * Mathf.Deg2Rad;     

        Vector2 CirclePos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        Vector2 spawnPos = (Vector2)playerTransform.position + CirclePos * circleSpawnDistance;

        return spawnPos;
    }
    public List<Vector2> LinePosition(Vector2 point, int spawnCount, float angle)
    {
        if (playerTransform == null) return null;

        List<Vector2> lines = new List<Vector2>();

        int padding = 3;

        for(int i = 0; i < spawnCount; i++)
        {
            Vector2 pos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * (padding * i);
            lines.Add(point + pos);
        }

        return lines;
    }


}
