using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MonsterSpawnFactory : MonoBehaviour, IPlayerObserver
{
    private Transform playerTransform;
    private PlayerSubject playerSubject;

    private float defaultDistance = 10f;
    private float circleSpawnDistance = 10f;
    private float lineSpawnDistance = 5f;

    private MonsterData currentMonsterData = null;

    private void Awake()
    {
        playerSubject = GameObject.FindWithTag("PlayerSubject").GetComponent<PlayerSubject>();
        playerSubject.AddObserver(this);
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

    public void CreateMonster(MonsterData monsterData, int spawnCount, int spawnType)
    {
        if (monsterData == null) return;

        currentMonsterData = monsterData;

        switch (spawnType)
        {
            // 랜덤 생성
            case 1:
                for (int i = 0; i < spawnCount; i++)
                {
                    Addressables.InstantiateAsync(monsterData.Monster_Prefab.ToString(),
                        RandomPosition(defaultDistance), Quaternion.identity).Completed += MonsterAddComponent;
                }
                break;
            // 직선 생성
            case 2:
                var lineList = LinePosition(RandomPosition(lineSpawnDistance), spawnCount, 0f);

                for (int i = 0; i < spawnCount; i++)
                {
                    Addressables.InstantiateAsync(monsterData.Monster_Prefab.ToString(),
                        lineList[i], Quaternion.identity).Completed += MonsterAddComponent;
                }
                break;
            // 원 생성
            case 3:
                for (int i = 0; i < spawnCount; i++)
                {
                    Addressables.InstantiateAsync(monsterData.Monster_Prefab.ToString(),
                        CirclePosition(spawnCount, i), Quaternion.identity).Completed += MonsterAddComponent;
                }
                break;
        }
    }

    public void MonsterAddComponent(AsyncOperationHandle<GameObject> op)
    {
        GameObject monster = op.Result;

        var typeData = DataTableManager.Instance.Get<MonsterTypeTable>
            (DataTableManager.monsterType).GetMonsterTypeData(currentMonsterData.Monster_Type.ToString());

        var list = typeData.GetSkillDatas();

        // 움직임 스크립트 추가
        var ccm = monster.AddComponent<ConstantChaseMove>();
        ccm.Initialize(playerSubject);

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == -1) break;

            var skillData = DataTableManager.Instance.Get<MonsterSkillTable>
                (DataTableManager.monsterSkill).GetMonsterSkillData(list[i].ToString());

            switch (skillData.Skill_Id) 
            {
                // 근접 공격
                case 300001:
                    
                    break;
                // 원거리 공격
                case 300002:

                    break;
            }
        }
    }

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

    public void IObserverUpdate()
    {
        playerTransform = playerSubject.GetPlayerTransform;
    }
}
