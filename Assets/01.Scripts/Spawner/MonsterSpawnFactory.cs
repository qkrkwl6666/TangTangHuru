using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class MonsterSpawnFactory : MonoBehaviour, IPlayerObserver
{
    private Transform playerTransform;
    private PlayerSubject playerSubject;

    private float defaultDistance = 20f;
    private float circleSpawnDistance = 7f;
    private float lineSpawnDistance = 5f;

    //private MonsterData currentMonsterData = null;
    private int spawnType = 1;
    private int spawnCount = 1;

    // 오브젝트 풀 관련 변수들
    public List<GameObject> monsters = new List<GameObject>();
    public Dictionary<int, IObjectPool<GameObject>> monsterPools = new Dictionary<int, IObjectPool<GameObject>>();
    public int maxMonster = 200; // 필드 최대 몬스터 수

    //장착할 HP바 프리팹
    public GameObject hpBarObj;

    // 보스 장애물 관련 변수들
    private int obstaclesCount = 90; // 장애물 개수
    private float obstaclesRadius = 20f;

    Vector2 bossSpawnPosition = Vector2.zero;

    private InGameUI gameUI;
    private float currentSpawnDistance = 0f;

    private float mapMaxSize = 250f;
    private MonsterSkeletonSharing monsterSkeletonSharing;

    private void Awake()
    {
        playerSubject = GameObject.FindWithTag("PlayerSubject").GetComponent<PlayerSubject>();
        playerSubject.AddObserver(this);

        gameUI = GameObject.FindWithTag("InGameUI").GetComponent<InGameUI>();

        monsterSkeletonSharing = GameObject.FindWithTag("MonsterSkeletonSharing").GetComponent<MonsterSkeletonSharing>();
    }

    private void Update()
    {

    }

    public void CreateMonster(MonsterData monsterData, int spawnCount, int spawnType, float spawnDistance)
    {
        if (monsterData == null || monsters.Count >= maxMonster) return;

        this.spawnType = spawnType;
        this.spawnCount = spawnCount;
        currentSpawnDistance = spawnDistance;

        //Debug.Log(monsterData.Monster_Prefab);
        var opHandle = Addressables.LoadAssetAsync<GameObject>(monsterData.Monster_Prefab.ToString());

        opHandle.Completed += (op) => MonsterInstantiate(op, monsterData, spawnCount, spawnType);
    }

    public void MonsterInstantiate(AsyncOperationHandle<GameObject> op, MonsterData monsterData, int spawnCount, int spawnType)
    {
        GameObject monsterPrefab = op.Result;

        if (!monsterPools.ContainsKey(monsterData.Monster_ID))
        {
            monsterPools[monsterData.Monster_ID] = new ObjectPool<GameObject>
                (() =>
                {
                    var go = Instantiate(monsterPrefab);

                    var skeletonRenderer = go.GetComponentInChildren<SkeletonRenderer>();

                    monsterSkeletonSharing.AddSkeletonRenderers(monsterData.Monster_Prefab.ToString(), skeletonRenderer);

                    var monsterView = go.AddComponent<MonsterView>();
                    monsterView.skeletonRenderer = skeletonRenderer;

                    var monsterScript = go.GetComponent<Monster>();
                    monsterScript.SetPool(monsterPools[monsterData.Monster_ID]);

                    var adp = go.AddComponent<AdjustMonsterPosition>(); // 위치 조정 스크립트 추가
                    adp.Initialize(playerSubject);

                    monsterScript.Initialize(playerSubject, monsterData);

                    // 몬스터 컨트롤러 생성
                    var mc = go.AddComponent<MonsterController>();
                    mc.MoveSpeed = monsterData.Monster_MoveSpeed;

                    if (monsterData.Monster_Skill_Id == -1) return go;

                    var skillData = DataTableManager.Instance.Get<MonsterSkillTable>
                    (DataTableManager.monsterSkill).GetMonsterSkillData(monsterData
                         .Monster_Skill_Id.ToString());

                    MonsterSkillAddComponent(go, skillData);

                    return go;
                },
                (x) =>
                {
                    monsters.Add(x);
                    var hp = x.GetComponent<Monster>();
                    if (hp.hpBar == null)
                    {
                        var hpCanvas = Instantiate(hpBarObj, x.transform);
                        hp.hpBar = hpCanvas.GetComponentInChildren<Slider>();
                    }
                    hp.AwakeHealth();
                    hp.SetHpBar();
                    x.SetActive(true);
                },
                (x) =>
                {
                    monsters.Remove(x);
                    x.SetActive(false);
                },
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
                    GameObject monster = monsterPools[monsterData.Monster_ID].Get();
                    monster.transform.position = RandomPosition();
                    monster.transform.rotation = Quaternion.identity;
                    monster.GetComponent<Monster>().SetHpBar();
                }
                break;
            // 직선 생성
            case 2:
                var lineList = LinePosition(RandomPosition(), spawnCount, 0f);

                for (int i = 0; i < spawnCount; i++)
                {
                    GameObject monster = monsterPools[monsterData.Monster_ID].Get();
                    monster.transform.position = lineList[i];
                    monster.transform.rotation = Quaternion.identity;
                    monster.GetComponent<Monster>().SetHpBar();
                }
                break;
            // 원 생성
            case 3:
                for (int i = 0; i < spawnCount; i++)
                {
                    GameObject monster = monsterPools[monsterData.Monster_ID].Get();
                    monster.transform.position = CirclePosition(spawnCount, i);
                    monster.transform.rotation = Quaternion.identity;
                    monster.GetComponent<Monster>().SetHpBar();
                }
                break;
        }

    }

    //async Task<SkeletonAnimation> GetMonsterWithAnimationAsync(MonsterData monsterData)
    //{
    //    var animation = await monsterSkeletonSharing.GetSkeletonAnimationAsync(monsterData.Monster_Prefab.ToString());

    //    return animation;
    //}

    // Todo : 몬스터 테이블 수정 
    public void MonsterSkillAddComponent(GameObject monster, MonsterSkillData skillData)
    {
        switch (skillData.Skill_Id)
        {
            // 스킬 추가 
            case 300001:
                //var attack = monster.AddComponent<HitOnStay>();
                //attack.Damage = 5f;
                //attack.PierceCount = 999f;
                //attack.CriticalChance = -1f;
                //attack.AttackRate = 2.5f;
                //attack.AttackableLayer = LayerMask.GetMask("Player");
                break;
            case 300002:
                break;
        }
    }

    public void MonsterAllDead()
    {

        foreach (var monster in monsters)
        {
            monster.SetActive(false);
        }

        monsters.Clear();
    }

    public void CreateBoss(BossData bossData)
    {
        Vector2 bossPos = (Random.insideUnitCircle.normalized * 3) + bossSpawnPosition;

        Addressables.InstantiateAsync(bossData.Boss_Prefab, bossPos, Quaternion.identity).Completed +=
            (x) =>
            {
                var monsterGo = x.Result;
                monsterGo.GetComponent<Boss>().Initialize(playerSubject, bossData);

                gameUI.SetActiveBossHpBar(true);
            };
    }

    public void BossWallSpawn()
    {
        var wallGo = new GameObject("BossWall");

        bossSpawnPosition = wallGo.transform.position = playerTransform.position;

        Addressables.LoadAssetAsync<GameObject>(Defines.obstacles).Completed += (x) =>
        {
            for (int i = 0; i < obstaclesCount; i++)
            {
                float angle = ((360 / obstaclesCount) * i) * Mathf.Deg2Rad;

                Vector2 pos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized * obstaclesRadius;

                Instantiate(x.Result, (Vector2)playerTransform.position + pos, Quaternion.identity, wallGo.transform);
            }
        };
    }

    public static Vector2 RandomPosition(Transform playerTransform, float distance)
    {
        if (playerTransform == null) return Vector2.zero;

        // Todo : 임시 처리 하드코딩 변경 해야함

        if (playerTransform.position.x >= Defines.maxMapSize || playerTransform.position.x <= -Defines.maxMapSize
            || playerTransform.position.y >= Defines.maxMapSize || playerTransform.position.y <= -Defines.maxMapSize)
        {
            playerTransform.transform.position = Vector2.zero;
        }

        Vector2 randomCirclePos;
        Vector2 spawnPos;

        while (true)
        {
            randomCirclePos = UnityEngine.Random.insideUnitCircle.normalized * distance;
            spawnPos = (Vector2)playerTransform.position + randomCirclePos;

            if (spawnPos.x >= Defines.maxMapSize || spawnPos.x <= -Defines.maxMapSize 
                || spawnPos.y >= Defines.maxMapSize || spawnPos.y <= -Defines.maxMapSize)
            {
                // x 좌표 조정
                if (spawnPos.x >= Defines.maxMapSize)
                    spawnPos.x = Defines.maxMapSize - 2;
                else if (spawnPos.x <= -Defines.maxMapSize)
                    spawnPos.x = -Defines.maxMapSize + 2;

                // y 좌표 조정
                if (spawnPos.y >= Defines.maxMapSize)
                    spawnPos.y = Defines.maxMapSize - 2;
                else if (spawnPos.y <= -Defines.maxMapSize)
                    spawnPos.y = -Defines.maxMapSize + 2;
            }
            else break;
        }

        Debug.Log(spawnPos);
        return spawnPos;
    }

    public Vector2 RandomPosition()
    {
        if (playerTransform == null) return Vector2.zero;
        Vector2 randomCirclePos;
        Vector2 spawnPos;

        // Todo : 임시 처리 하드코딩 변경 해야함
        if (playerTransform.position.x >= mapMaxSize || playerTransform.position.x <= -mapMaxSize
            || playerTransform.position.y >= mapMaxSize || playerTransform.position.y <= -mapMaxSize)
        {
            playerTransform.transform.position = Vector2.zero;
        }

        while (true)
        {
            float randomDistance = Random.Range(currentSpawnDistance / 2, currentSpawnDistance);

            randomCirclePos = UnityEngine.Random.insideUnitCircle.normalized * randomDistance;
            spawnPos = (Vector2)playerTransform.position + randomCirclePos;

            if (spawnPos.x >= mapMaxSize || spawnPos.x <= -mapMaxSize 
                || spawnPos.y >= mapMaxSize || spawnPos.y <= -mapMaxSize)
            {
                // x 좌표 조정
                if (spawnPos.x >= mapMaxSize)
                    spawnPos.x = mapMaxSize - 2;
                else if (spawnPos.x <= -mapMaxSize)
                    spawnPos.x = -mapMaxSize + 2;

                // y 좌표 조정
                if (spawnPos.y >= mapMaxSize)
                    spawnPos.y = mapMaxSize - 2;
                else if (spawnPos.y <= -mapMaxSize)
                    spawnPos.y = -mapMaxSize + 2;
            }
            else break;
        }

        Debug.Log(spawnPos);
        return spawnPos;
    }

    public Vector2 CirclePosition(int spawnCount, int currentSpawnCount)
    {
        if (playerTransform == null) return Vector2.zero;

        float angle = ((360 / spawnCount) * currentSpawnCount) * Mathf.Deg2Rad;

        Vector2 CirclePos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        Vector2 spawnPos = (Vector2)playerTransform.position + CirclePos * currentSpawnDistance;

        if (spawnPos.x >= mapMaxSize || spawnPos.x <= -mapMaxSize
                || spawnPos.y >= mapMaxSize || spawnPos.y <= -mapMaxSize)
        {
            // x 좌표 조정
            if (spawnPos.x >= mapMaxSize)
                spawnPos.x = mapMaxSize - 2;
            else if (spawnPos.x <= -mapMaxSize)
                spawnPos.x = -mapMaxSize + 2;

            // y 좌표 조정
            if (spawnPos.y >= mapMaxSize)
                spawnPos.y = mapMaxSize - 2;
            else if (spawnPos.y <= -mapMaxSize)
                spawnPos.y = -mapMaxSize + 2;
        }

        return spawnPos;
    }
    public List<Vector2> LinePosition(Vector2 point, int spawnCount, float angle)
    {
        if (playerTransform == null) return null;

        List<Vector2> lines = new List<Vector2>();

        int padding = 3;

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 pos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * (padding * i);
            Vector2 spawnPos = point + pos;

            if (spawnPos.x >= mapMaxSize || spawnPos.x <= -mapMaxSize 
                || spawnPos.y >= mapMaxSize || spawnPos.y <= -mapMaxSize)
            {
                // x 좌표 조정
                if (spawnPos.x >= mapMaxSize)
                    spawnPos.x = mapMaxSize - 2;
                else if (spawnPos.x <= -mapMaxSize)
                    spawnPos.x = -mapMaxSize + 2;

                // y 좌표 조정
                if (spawnPos.y >= mapMaxSize)
                    spawnPos.y = mapMaxSize - 2;
                else if (spawnPos.y <= -mapMaxSize)
                    spawnPos.y = -mapMaxSize + 2;
            }

            lines.Add(spawnPos);
        }

        return lines;
    }

    public void IObserverUpdate()
    {
        playerTransform = playerSubject.GetPlayerTransform;
    }
}
