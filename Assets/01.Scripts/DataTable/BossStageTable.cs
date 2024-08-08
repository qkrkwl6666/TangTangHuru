using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine.AddressableAssets;
using TextAsset = UnityEngine.TextAsset;

public class BossStageData
{
    public int Stage { get; set; }
    public int Middle1_Id { get; set; }
    public int Middle2_Id { get; set; }
    public int Middle3_Id { get; set; }
    public int Boss_Id { get; set; }
}

public class BossStageTable : DataTable
{
    public Dictionary<string, BossStageData> bossStageTable { get; private set; } = new();

    public BossStageData GetBossData(string stage)
    {
        if (!bossStageTable.ContainsKey(stage)) return null;

        return bossStageTable[stage];
    }

    public override void Load(string name, Action tableLoaded)
    {
        Addressables.LoadAssetAsync<TextAsset>(name).Completed += (textAsset) =>
        {
            using (var reader = new StringReader(textAsset.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csvReader.GetRecords<BossStageData>();

                foreach (var record in records)
                {
                    bossStageTable.Add(record.Stage.ToString(), record);
                }
            }

            tableLoaded?.Invoke();
        };
    }
}
