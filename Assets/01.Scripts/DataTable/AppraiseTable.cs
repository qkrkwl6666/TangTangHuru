using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine.AddressableAssets;
using TextAsset = UnityEngine.TextAsset;

public class AppraiseData
{
    public int Id { get; set; }
    public int Gold { get; set; }
    public int Level1_weapon { get; set; }
    public int Level2_weapon { get; set; }
    public int Level3_weapon { get; set; }
    public int Level4_weapon { get; set; }
    public int Level5_weapon { get; set; }

    public int Level1_helmet { get; set; }
    public int Level2_helmet { get; set; }
    public int Level3_helmet { get; set; }
    public int Level4_helmet { get; set; }
    public int Level5_helmet { get; set; }

    public int Level1_armor { get; set; }
    public int Level2_armor { get; set; }
    public int Level3_armor { get; set; }
    public int Level4_armor { get; set; }
    public int Level5_armor { get; set; }

    public int Level1_shoes { get; set; }
    public int Level2_shoes { get; set; }
    public int Level3_shoes { get; set; }
    public int Level4_shoes { get; set; }
    public int Level5_shoes { get; set; }

    public List<(ItemType type, ItemTier tier, float prob)> GetAppraiseData()
    {
        List<(ItemType type, ItemTier tier, float prob)> list = new();

        list.Add((ItemType.Weapon, ItemTier.Normal, Level1_weapon));
        list.Add((ItemType.Weapon, ItemTier.Rare, Level2_weapon));
        list.Add((ItemType.Weapon, ItemTier.Epic, Level3_weapon));
        list.Add((ItemType.Weapon, ItemTier.Unique, Level4_weapon));
        list.Add((ItemType.Weapon, ItemTier.Legendary, Level5_weapon));

        list.Add((ItemType.Helmet, ItemTier.Normal, Level1_helmet));
        list.Add((ItemType.Helmet, ItemTier.Rare, Level2_helmet));
        list.Add((ItemType.Helmet, ItemTier.Epic, Level3_helmet));
        list.Add((ItemType.Helmet, ItemTier.Unique, Level4_helmet));
        list.Add((ItemType.Helmet, ItemTier.Legendary, Level5_helmet));

        list.Add((ItemType.Armor, ItemTier.Normal, Level1_armor));
        list.Add((ItemType.Armor, ItemTier.Rare, Level2_armor));
        list.Add((ItemType.Armor, ItemTier.Epic, Level3_armor));
        list.Add((ItemType.Armor, ItemTier.Unique, Level4_armor));
        list.Add((ItemType.Armor, ItemTier.Legendary, Level5_armor));

        list.Add((ItemType.Shose, ItemTier.Normal, Level1_shoes));
        list.Add((ItemType.Shose, ItemTier.Rare, Level2_shoes));
        list.Add((ItemType.Shose, ItemTier.Epic, Level3_shoes));
        list.Add((ItemType.Shose, ItemTier.Unique, Level4_shoes));
        list.Add((ItemType.Shose, ItemTier.Legendary, Level5_shoes));

        return list;
    }

}

public class AppraiseTable : DataTable
{
    public Dictionary<string, AppraiseData> appraiseTable { get; private set; } = new();

    public AppraiseData GetData(string id)
    {
        if (!appraiseTable.ContainsKey(id)) return null;

        return appraiseTable[id];
    }

    public override void Load(string name, Action tableLoaded)
    {
        Addressables.LoadAssetAsync<TextAsset>(name).Completed += (textAsset) =>
        {
            using (var reader = new StringReader(textAsset.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csvReader.GetRecords<AppraiseData>();

                foreach (var record in records)
                {
                    appraiseTable.Add(record.Id.ToString(), record);
                }
            }

            tableLoaded?.Invoke();
        };
    }
}
