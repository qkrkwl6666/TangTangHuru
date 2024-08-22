using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine.AddressableAssets;
using TextAsset = UnityEngine.TextAsset;

public class ItemData
{
    public int Item_Id { get; set; }
    public string Name_Id { get; set; }
    public string Prefab_Id { get; set; }
    public string Desc_Id { get; set; }
    public string Texture_Id { get; set; }
    public string Spine_Id { get; set; }
    public string Outline { get; set; }
    public int Item_Type { get; set; }
    public int Item_Tier { get; set; }
    public float Damage { get; set; }
    public float Defense { get; set; }
    public float Hp { get; set; }
    public float Dodge { get; set; }
    public float Damagecal { get; set; }
    public float CoolDown { get; set; }
    public float CriticalChance { get; set; }
    public float Criticaldam { get; set; }

    // 무기 강화 수치
    public float UpDamage { get; set; }
    public float UpCoolDown { get; set; }
    public float UpCriticalChance { get; set; }
    public float UpCriticalDam { get; set; }

    // 무기 방어구 강화 단계
    public int CurrentUpgrade = 0;

    public float TierUp_Exp { get; set; }
    public float TierUp_NeedExp { get; set; }

    //방어구 세트 정보
    public int SetType { get; set; }

    //가격
    public int Price { get; set; }


    public ItemData DeepCopy()
    {
        ItemData newItemData = new ItemData();

        newItemData.Item_Id = this.Item_Id;
        newItemData.Name_Id = this.Name_Id;
        newItemData.Prefab_Id = this.Prefab_Id;
        newItemData.Desc_Id = this.Desc_Id;
        newItemData.Texture_Id = this.Texture_Id;
        newItemData.Spine_Id = this.Spine_Id;
        newItemData.Outline = this.Outline;
        newItemData.Item_Type = this.Item_Type;
        newItemData.Item_Tier = this.Item_Tier;
        newItemData.Damage = this.Damage;
        newItemData.Defense = this.Defense;
        newItemData.Hp = this.Hp;
        newItemData.Dodge = this.Dodge;
        newItemData.Damagecal = this.Damagecal;
        newItemData.CoolDown = this.CoolDown;
        newItemData.CriticalChance = this.CriticalChance;
        newItemData.Criticaldam = this.Criticaldam;
        newItemData.CurrentUpgrade = this.CurrentUpgrade;
        newItemData.TierUp_Exp = this.TierUp_Exp;
        newItemData.TierUp_NeedExp = this.TierUp_NeedExp;
        newItemData.SetType = this.SetType;
        newItemData.Price = this.Price;


        return newItemData;
    }
}

public class ItemTable : DataTable
{
    public Dictionary<string, ItemData> itemTable { get; private set; } = new();

    public ItemData GetItemData(string item_Id)
    {
        if (!itemTable.ContainsKey(item_Id)) return null;

        return itemTable[item_Id];
    }

    public ItemData GetItemData(ItemType itemType, ItemTier itemTier)
    {
        foreach (var item in itemTable.Values) 
        {
            if (item.Item_Type == (int)itemType && item.Item_Tier == (int)itemTier)
            {
                return item;
            }
        }

        return null;
    }

    public List<ItemData> GetItemDatas(ItemType itemType, ItemTier itemTier)
    {
        List<ItemData> itemDatas = new List<ItemData>();

        foreach (var item in itemTable.Values)
        {
            if (item.Item_Type == (int)itemType && item.Item_Tier == (int)itemTier)
            {
                itemDatas.Add(item);
            }
        }

        return itemDatas;
    }

    public List<ItemData> GetItemsbySet(ItemTier itemTier, int setIndex)
    {
        List<ItemData> itemDatas = new List<ItemData>();

        foreach (var item in itemTable.Values)
        {
            if (item.Item_Tier == (int)itemTier && item.SetType == setIndex)
            {
                itemDatas.Add(item);
            }
        }

        return itemDatas;
    }


    public override void Load(string name, Action tableLoaded)
    {
        Addressables.LoadAssetAsync<TextAsset>(name).Completed += (textAsset) =>
        {
            using (var reader = new StringReader(textAsset.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csvReader.GetRecords<ItemData>();

                foreach (var record in records)
                {
                    itemTable.Add(record.Item_Id.ToString(), record);
                }
            }

            tableLoaded?.Invoke();
        };
    }

}
