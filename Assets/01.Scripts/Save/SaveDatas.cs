using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SaveDatas 
{
    public int Version { get; protected set; }
    public abstract SaveData VersionUp();
}

public class SaveDataV1 : SaveDatas
{
    public int Gold { get; set; } = 100;
    public int Diamond { get; set; } = 100;
    public int CurrentStage { get; set; } = 21;

    // 전체 아이템 컨테이너
    public SortedDictionary<ItemType, SortedDictionary<ItemTier, List<Item>>> allItem = new();

    // 플레이어가 가지고있는 아이템 컨테이너 장비 
    public Dictionary<PlayerEquipment, Item> playerEquipment = new ();

    public SaveDataV1()
    {
        Version = 1;
    }

    public override SaveData VersionUp()
    {
        //SaveDataV2 saveData = new SaveDataV2();
        //saveData.Gold = Gold;

        return null;
    }
}