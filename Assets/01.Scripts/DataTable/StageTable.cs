using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine.AddressableAssets;
using TextAsset = UnityEngine.TextAsset;

public class StageData
{
    public int Stage { get; set; }
    public string Title { get; set; }
    public string Desc { get; set; }
    public string Texture { get; set; }
}

public class StageTable : DataTable
{
    public Dictionary<string, StageData> stageTable { get; private set; } = new();

    public static Action OnStageSelectionUIInit;

    public StageData GetData(int stage)
    {
        if(stageTable.ContainsKey(stage.ToString()))
        {
            return stageTable[stage.ToString()];
        }
        
        return null;
    }

    public override void Load(string name)
    {
        Addressables.LoadAssetAsync<TextAsset>(name).Completed += (textAsset) =>
        {
            using (var reader = new StringReader(textAsset.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csvReader.GetRecords<StageData>();

                foreach (var record in records)
                {
                    stageTable.Add(record.Stage.ToString(), record);
                }
            }

            OnStageSelectionUIInit?.Invoke();

        };
    }
}
