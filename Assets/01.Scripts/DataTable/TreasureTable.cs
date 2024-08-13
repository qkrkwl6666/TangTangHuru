using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine.AddressableAssets;
using TextAsset = UnityEngine.TextAsset;

public class TreasureData
{
    public int Stage { get; set; }
    public int Item1_Id { get; set; }
    public float Item1_Prob { get; set; }
    public int Item2_Id { get; set; }
    public float Item2_Prob { get; set; }
    public int Item3_Id { get; set; }
    public float Item3_Prob { get; set; }
    public int Item4_Id { get; set; }
    public float Item4_Prob { get; set; }
    public int Item5_Id { get; set; }
    public float Item5_Prob { get; set; }

    public int Min_Re_Stone { get; set; }
    public int Max_Re_Stone { get; set; }

    public List<(int id, float prob)> GetEqupStones()
    {
        List<(int, float)> list = new();

        list.Add((Item1_Id, Item1_Prob));
        list.Add((Item2_Id, Item2_Prob));
        list.Add((Item3_Id, Item3_Prob));
        list.Add((Item4_Id, Item4_Prob));
        list.Add((Item5_Id, Item5_Prob));

        return list;
    }
}

public class TreasureTable : DataTable
{
    public Dictionary<string, TreasureData> treasureTable { get; private set; } = new();

    public TreasureData GetTreasure(string name)
    {
        if (!treasureTable.ContainsKey(name)) return null;

        return treasureTable[name];
    }

    public override void Load(string name, Action tableLoaded)
    {
        Addressables.LoadAssetAsync<TextAsset>(name).Completed += (textAsset) =>
        {
            using (var reader = new StringReader(textAsset.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csvReader.GetRecords<TreasureData>();

                foreach (var record in records)
                {
                    treasureTable.Add(record.Stage.ToString(), record);
                }
            }

            tableLoaded?.Invoke();
        };
    }
}
