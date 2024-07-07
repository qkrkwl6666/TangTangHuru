using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using CsvHelper;
using System.Globalization;
using System.IO;
using TextAsset = UnityEngine.TextAsset;

public class MonsterData
{
    public int Monster_ID { get; set; }
    public int Monster_Prefab { get; set; }
    public int Monster_Type { get; set; }
    public int Monster_Hp { get; set; }
    public int Monster_Move { get; set; }
    public int Monster_Damage { get; set; }
    public int Monster_Exp { get; set; }
    public int Monster_Gold { get; set; }
}

public class MonsterTable : DataTable
{
    public Dictionary<string, MonsterData> monsterTable { get; private set; } = new();

    public override void Load(string name)
    {
        Addressables.LoadAssetAsync<TextAsset>(name).Completed += (textAsset) =>
        {
            using (var reader = new StringReader(textAsset.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csvReader.GetRecords<MonsterData>();

                foreach(var record in records)
                {
                    monsterTable.Add(record.Monster_ID.ToString(), record);
                }
            }
        };
    }
}
