using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MonsterSpawnFactory : MonoBehaviour, IPlayerObserver
{
    private Transform playerTransform;
    private PlayerSubject playerSubject;

    private float defaultDistance = 10f;
    private float circleSpawnDistance = 7f;
    private float lineSpawnDistance = 5f;

    private MonsterData currentMonsterData = null;
    private int spawnType = 1;
    private int spawnCount = 1;

    // 오브젝트 풀 관련 변수들
    public List<GameObject> monsters = new List<GameObject>();
    public Dictionary<int, IObjectPool<GameObject>> monsterPools = new Dictionary<int, IObjectPool<GameObject>>();
    public int maxMonster = 200; // 필드 최대 몬스터 수

    private void Awake()
    {
        playerSubject = GameObject.FindWithTag("PlayerSubject").GetComponent<PlayerSubject>();
        playerSubject.AddObserver(this);
    }

    private void Update()
    {
        // if (Input.GetKeyUp(KeyCode.F1))
        // {
        //     int index = Random.Range(0, monsters.Count);
        //     var go = monsters[index];
        // 
        //     if (go.activeSelf)
        //     {
        //         go.GetComponent<Monster>().Die();
        //     }
        // }

        // if (Input.GetKeyUp(KeyCode.F2))
        // {
        //     CreateMonster(DataTableManager.Instance.Get<MonsterTable>
        //         (DataTableManager.monster).GetMonsterData("100002"), 10, 2);
        // }
        // 
        // if (Input.GetKeyUp(KeyCode.F3))
        // {
        //     CreateMonster(DataTableManager.Instance.Get<MonsterTable>
        //         (DataTableManager.monster).GetMonsterData("100003"),10, 3);
        // }

    }

    public void CreateMonster(MonsterData monsterData, int spawnCount, int spawnType)
    {
        if (monsterData == null || monsters.Count >= maxMonster) return;

        currentMonsterData = monsterData;

        this.spawnType = spawnType;
        this.spawnCount = spawnCount;

        var opHandle = Addressables.LoadAssetAsync<GameObject>(monsterData.Monster_Prefab.ToString());

        opHandle.Completed += MonsterInstantiate;
    }

    public void MonsterInstantiate(AsyncOperationHandle<GameObject> op)
    {
        GameObject monsterPrefab = op.Result;

       if (!monsterPools.ContainsKey(currentMonsterData.Monster_ID))
       {
           monsterPools[currentMonsterData.Monster_ID] = new ObjectPool<GameObject>
               ( () => 
               {
                   var go = Instantiate(monsterPrefab);
                   var monsterScript = go.GetComponent<Monster>();
                   monsterScript.SetPool(monsterPools[currentMonsterData.Monster_ID]);
                   var ccm = go.AddComponent<ConstantChaseMove>(); // 움직임 스크립트 추가
                   var adp = go.AddComponent<AdjustMonsterPosition>(); // 위치 조정 스크립트 추가
                   adp.Initialize(playerSubject);
                   ccm.Initialize(playerSubject);
                   monsterScript.Initialize(playerSubject, currentMonsterData);

                   var skillData = DataTableManager.Instance.Get<MonsterSkillTable>
                   (DataTableManager.monsterSkill).GetMonsterSkillData(currentMonsterData
                        .Monster_Skill_Id.ToString());

                   MonsterSkillAddComponent(go, skillData);

                   return go; 
               },
               (x) => 
               { 
                   monsters.Add(x);
                   x.SetActive(true);
               },
               (x) => 
               {
                   monsters.Remove(x);
                   x.SetActive(false); 
               } ,
               (x) => Destroy(x.gameObject),
               true,
               10, 200);
       }
       
       switch (spawnType)
       {
           // 랜덤 생성
           case 1:
               for (int i = 0; i < spawnCount; i++)
               {
                   var monster = monsterPools[currentMonsterData.Monster_ID].Get();
                   monster.transform.position = RandomPosition(defaultDistance);
                   monster.transform.rotation = Quaternion.identity;
               }
               break;
           // 직선 생성
           case 2:
               var lineList = LinePosition(RandomPosition(lineSpawnDistance), spawnCount, 0f);
       
               for (int i = 0; i < spawnCount; i++)
               {
                   var monster = monsterPools[currentMonsterData.Monster_ID].Get();
                   monster.transform.position = lineList[i];
                   monster.transform.rotation = Quaternion.identity;
               }
               break;
           // 원 생성
           case 3:
               for (int i = 0; i < spawnCount; i++)
               {
                   var monster = monsterPools[currentMonsterData.Monster_ID].Get();
                   monster.transform.position = CirclePosition(spawnCount, i);
                   monster.transform.rotation = Quaternion.identity;
               }
               break;
       }

    }

    // Todo : 몬스터 테이블 수정 
    public void MonsterSkillAddComponent(GameObject monster , MonsterSkillData skillData)
    {
        switch (skillData.Skill_Id)
        {
            // 스킬 추가 
            case 300001:
                var attack = monster.AddComponent<HitOnStay>();
                attack.Damage = 5f;
                attack.PierceCount = 999f;
                attack.CriticalChance = -1f;
                attack.AttackRate = 0.5f;
                attack.AttackableLayer = LayerMask.GetMask("Player");
                break;
            case 300002:
                break;
        }
    }

    public void MonsterAllDead()
    {

        foreach(var monster in monsters)
        {
            monster.SetActive(false);
        }

        monsters.Clear();
    }

    public void CreateBoss(BossData bossData)
    {
 
        Addressables.InstantiateAsync(bossData.Boss_Prefab).Completed += 
            (x) => 
            {
                var monsterGo = x.Result;
                monsterGo.GetComponent<Boss>().Initialize(playerSubject, bossData);
            };
    }

    public static Vector2 RandomPosition(Transform playerTransform, float distance)
    {
        if (playerTransform == null) return Vector2.zero;

        Vector2 randomCirclePos = UnityEngine.Random.insideUnitCircle.normalized * distance;
        Vector2 spawnPos = (Vector2)playerTransform.position + randomCirclePos;

        return spawnPos;
    }

    public Vector2 RandomPosition(float distance)
    {
        if (playerTransform == null) return Vector2.zero;

        Vector2 randomCirclePos = UnityEngine.Random.insideUnitCircle.normalized * distance;
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
