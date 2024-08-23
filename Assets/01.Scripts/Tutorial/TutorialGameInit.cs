using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TutorialGameInit : MonoBehaviour
{
    PlayerSubject playerSubject;
    public MonsterSkeletonSharing monsterSkeletonSharing;
    public ItemDetection itemDetection;

    public GameObject guardian;
    public List<GameObject> monsters = new();
    public GameObject treasureGo;

    private int maxMapSizeWidth = 70;
    private int maxMapSizeHeight = 70;

    private int currentSize = 0;
    private int wallSpace = 1;

    private void Awake()
    {

    }

    private void Start()
    {
        GameManager.Instance.isTutorialSceneEnd = true;

        // 벽 스폰
        SpawnWall();

        // 몬스터 스폰
        InitMonster();

        // 보물 상자 세팅
        InitTreasure();

        // 수호자 세팅
        InitGuardian();

        // 우물 배치 

        // 보물 찾기 효과 부여
    }

    public void InitMonster()
    {
        playerSubject = GameObject.FindWithTag("PlayerSubject").GetComponent<PlayerSubject>();

        MonsterData monsterData = DataTableManager.Instance.Get<MonsterTable>(DataTableManager.monster).GetMonsterData("100001");

        var copy = monsterData.GetDeepCopyMonsterData();

        copy.Monster_Hp = 1f;
        copy.Monster_Exp = 200f;

        foreach (var monster in monsters)
        {
            if (monster.GetComponentInChildren<SkeletonRenderer>() == null)
            {
                Debug.Log($"{monster.GetComponentInChildren<SkeletonRenderer>()} is null");
            }
            monsterSkeletonSharing.AddSkeletonRenderers("111", monster.GetComponentInChildren<SkeletonRenderer>());

            monster.GetComponent<Monster>().Initialize(playerSubject, copy);
        }
    }

    public void InitTreasure()
    {
        var treasure = treasureGo.AddComponent<Treasure>();

        var itemData = DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData("600001");

        // 자석
        Addressables.InstantiateAsync(Defines.magnet).Completed += (x) =>
        {
            treasure.AddItem(x.Result);
        };

        // 장비 원석 생성
        Addressables.InstantiateAsync("1_GemStone").Completed +=
        (x) =>
        {
            var stone = x.Result;
            var equip = stone.AddComponent<EquipmentGemstone>();
            equip.Init(itemData);

            treasure.AddItem(stone);
        };

        for (int i = 0; i < 5; i++)
        {
            Addressables.InstantiateAsync("Re_Stone").Completed +=
            (x) =>
            {
                var restoneGo = x.Result;
                var reStone = restoneGo.AddComponent<ReinforcedStone>();
                treasure.AddItem(restoneGo);
            };
        }

        itemDetection.SetTutorialTreasure(treasure);
    }

    public void InitGuardian()
    {
        BossData bossData = DataTableManager.Instance.Get<BossTable>(DataTableManager.boss).GetBossData("333001").GetDeepCopyBossData();

        bossData.Boss_Hp = 100000;
        bossData.Boss_Damage = 100000f;

        guardian.GetComponent<Boss>().Initialize(playerSubject, bossData);
        guardian.SetActive(false);
    }

    public void SpawnWall()
    {
        Transform walls = new GameObject("walls").transform;

        // 벽 생성 위
        currentSize = -maxMapSizeWidth;
        while (true)
        {
            Vector2 pos = new Vector2(currentSize, maxMapSizeWidth);
            Addressables.InstantiateAsync(Defines.obstacles, pos, Quaternion.identity, walls);

            if (currentSize >= maxMapSizeWidth)
            {
                break;
            }

            currentSize += wallSpace;
        }

        // 벽 생성 아래
        currentSize = -maxMapSizeWidth;
        while (true)
        {
            Vector2 pos = new Vector2(currentSize, -maxMapSizeWidth);
            Addressables.InstantiateAsync(Defines.obstacles, pos, Quaternion.identity, walls);

            if (currentSize >= maxMapSizeWidth)
            {
                break;
            }

            currentSize += wallSpace;
        }

        // 벽 생성 왼쪽
        currentSize = -maxMapSizeHeight;
        while (true)
        {
            Vector2 pos = new Vector2(-maxMapSizeWidth, currentSize);
            Addressables.InstantiateAsync(Defines.obstacles, pos, Quaternion.Euler(new Vector3(0f, 0f, 90f)), walls);

            if (currentSize >= maxMapSizeHeight)
            {
                break;
            }

            currentSize += wallSpace;
        }

        // 벽 생성 오른쪽
        currentSize = -maxMapSizeHeight;
        while (true)
        {
            Vector2 pos = new Vector2(maxMapSizeWidth, currentSize);
            Addressables.InstantiateAsync(Defines.obstacles, pos, Quaternion.Euler(new Vector3(0f, 0f, 90f)), walls);

            if (currentSize >= maxMapSizeHeight)
            {
                break;
            }

            currentSize += wallSpace;
        }
    }


}
