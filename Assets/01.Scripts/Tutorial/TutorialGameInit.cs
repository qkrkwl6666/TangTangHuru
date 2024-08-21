using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TutorialGameInit : MonoBehaviour
{
    public MonsterSkeletonSharing monsterSkeletonSharing;
    public ItemDetection itemDetection;

    public List<GameObject> monsters = new ();
    public GameObject treasureGo;

    private void Awake()
    {
        // ���� ����
        InitMonster();

        // ���� ���� ����
        InitTreasure();



        // �칰 ��ġ 

        // ���� ã�� ȿ�� �ο�
    }

    public void InitMonster()
    {
        PlayerSubject playerSubject = GameObject.FindWithTag("PlayerSubject").GetComponent<PlayerSubject>();

        MonsterData monsterData = DataTableManager.Instance.Get<MonsterTable>(DataTableManager.monster).GetMonsterData("100001");

        monsterData.Monster_Hp = 10f;
        monsterData.Monster_Exp = 150f;

        foreach (var monster in monsters)
        {
            monsterSkeletonSharing.AddSkeletonRenderers("111", monster.GetComponentInChildren<SkeletonRenderer>());
            monster.GetComponent<Monster>().Initialize(playerSubject, monsterData);
        }
    }

    public void InitTreasure()
    {
        var treasure = treasureGo.AddComponent<Treasure>();

        var itemData = DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData("600001");

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


}
