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

        // �� ����
        SpawnWall();

        // ���� ����
        InitMonster();

        // ���� ���� ����
        InitTreasure();

        // ��ȣ�� ����
        InitGuardian();

        // �칰 ��ġ 

        // ���� ã�� ȿ�� �ο�
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

        // �ڼ�
        Addressables.InstantiateAsync(Defines.magnet).Completed += (x) =>
        {
            treasure.AddItem(x.Result);
        };

        // ��� ���� ����
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

        // �� ���� ��
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

        // �� ���� �Ʒ�
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

        // �� ���� ����
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

        // �� ���� ������
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
