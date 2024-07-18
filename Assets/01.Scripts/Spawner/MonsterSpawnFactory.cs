using System.Collections.Generic;
using TMPro;
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

    //private MonsterData currentMonsterData = null;
    private int spawnType = 1;
    private int spawnCount = 1;

    // ������Ʈ Ǯ ���� ������
    public List<GameObject> monsters = new List<GameObject>();
    public Dictionary<int, IObjectPool<GameObject>> monsterPools = new Dictionary<int, IObjectPool<GameObject>>();
    public int maxMonster = 200; // �ʵ� �ִ� ���� ��

    // �׽�Ʈ��

    public TextMeshProUGUI monsterCountText;

    private void Awake()
    {
        playerSubject = GameObject.FindWithTag("PlayerSubject").GetComponent<PlayerSubject>();
        playerSubject.AddObserver(this);
    }

    private void Update()
    {
        monsterCountText.text = monsters.Count.ToString();
    }

    public void CreateMonster(MonsterData monsterData, int spawnCount, int spawnType)
    {
        if (monsterData == null || monsters.Count >= maxMonster) return;

        this.spawnType = spawnType;
        this.spawnCount = spawnCount;

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
               ( () => 
               {
                   var go = Instantiate(monsterPrefab);
                   var monsterScript = go.GetComponent<Monster>();
                   monsterScript.SetPool(monsterPools[monsterData.Monster_ID]);
                   //var ccm = go.AddComponent<ConstantChaseMove>(); // ������ ��ũ��Ʈ �߰�
                   var adp = go.AddComponent<AdjustMonsterPosition>(); // ��ġ ���� ��ũ��Ʈ �߰�
                   adp.Initialize(playerSubject);
                   //ccm.Initialize(playerSubject);
                   monsterScript.Initialize(playerSubject, monsterData);

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
           // ���� ����
           case 1:
               for (int i = 0; i < spawnCount; i++)
               {
                   var monster = monsterPools[monsterData.Monster_ID].Get();
                   monster.transform.position = RandomPosition(defaultDistance);
                   monster.transform.rotation = Quaternion.identity;
               }
               break;
           // ���� ����
           case 2:
               var lineList = LinePosition(RandomPosition(lineSpawnDistance), spawnCount, 0f);
       
               for (int i = 0; i < spawnCount; i++)
               {
                   var monster = monsterPools[monsterData.Monster_ID].Get();
                   monster.transform.position = lineList[i];
                   monster.transform.rotation = Quaternion.identity;
               }
               break;
           // �� ����
           case 3:
               for (int i = 0; i < spawnCount; i++)
               {
                   var monster = monsterPools[monsterData.Monster_ID].Get();
                   monster.transform.position = CirclePosition(spawnCount, i);
                   monster.transform.rotation = Quaternion.identity;
               }
               break;
       }

    }

    // Todo : ���� ���̺� ���� 
    public void MonsterSkillAddComponent(GameObject monster , MonsterSkillData skillData)
    {
        switch (skillData.Skill_Id)
        {
            // ��ų �߰� 
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
