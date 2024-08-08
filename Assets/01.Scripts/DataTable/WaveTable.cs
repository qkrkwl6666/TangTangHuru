using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine.AddressableAssets;
using TextAsset = UnityEngine.TextAsset;

public class WaveData
{
    public int stage { get; set; }
    public int duration { get; set; }
    public int monster1_Id { get; set; }
    public int monster1_Count { get; set; }
    public int monster1_Duration { get; set; }
    public int spawn1_Type { get; set; }

    public int monster2_Id { get; set; }
    public int monster2_Count { get; set; }
    public int monster2_Duration { get; set; }
    public int spawn2_Type { get; set; }

    public int monster3_Id { get; set; }
    public int monster3_Count { get; set; }
    public int monster3_Duration { get; set; }
    public int spawn3_Type { get; set; }

    public float spawn_Distance { get; set; }
}

public class WaveTable : DataTable
{
    public Dictionary<string, List<WaveData>> waveTable { get; private set; } = new();

    public override void Load(string name, Action tableLoaded)
    {
        Addressables.LoadAssetAsync<TextAsset>(name).Completed += (textAsset) =>
        {
            using (var reader = new StringReader(textAsset.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csvReader.GetRecords<WaveData>();

                var waveList = records.ToList();

                for (int i = 0; i < waveList.Count; i++)
                {
                    if (!waveTable.ContainsKey(waveList[i].stage.ToString()))
                    {
                        waveTable.Add(waveList[i].stage.ToString(), new List<WaveData>());
                    }

                    waveTable[waveList[i].stage.ToString()].Add(waveList[i]);
                }
            }

            tableLoaded?.Invoke();
        };
    }
}
