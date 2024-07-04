using CsvHelper;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TextCore.Text;
using TextAsset = UnityEngine.TextAsset;

public class WaveData
{
    public int WaveID { get; set; }

    public int Nw1_ID { get; set; }
    public int Nw1_Count { get; set; }
    public int Nw1_Time { get; set; }

    public int Nw2_ID { get; set; }
    public int Nw2_Count { get; set; }
    public int Nw2_Time { get; set; }

    public int Nw3_ID { get; set; }
    public int Nw3_Count { get; set; }
    public int Nw3_Time { get; set; }

    public int Nw4_ID { get; set; }
    public int Nw4_Count { get; set; }
    public int Nw4_Time { get; set; }

    public int Nw5_ID { get; set; }
    public int Nw5_Count { get; set; }
    public int Nw5_Time { get; set; }
}

public class WaveTable : DataTable
{
    public Dictionary<string, WaveData> waveTable { get; private set; } = new();

    public override void Load(string name)
    {
        Addressables.LoadAssetAsync<TextAsset>(name).Completed += (textAsset) =>
        {
            using (var reader = new StringReader(textAsset.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csvReader.GetRecords<WaveData>();

                foreach (var record in records)
                {
                    waveTable.Add(record.WaveID.ToString(), record);
                }
            }
        };
    }
}
