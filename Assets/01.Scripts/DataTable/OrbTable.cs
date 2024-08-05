using CsvHelper;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;


public class OrbData
{
    public int Orb_ID { get; set; }
    public string Orb_Type { get; set; }
    public string Orb_Desc { get; set; }
    public string Orb_Texture { get; set; }
    public int Orb_Level { get; set; }
    public int Orb_Atk { get; set; }
    public int Orb_Hp { get; set; }
    public int Orb_Def { get; set; }
    public int Orb_Dodge { get; set; }
}


public class OrbTable : DataTable
{
    public Dictionary<string, OrbData> orbTable { get; private set; } = new();

    public OrbData GetOrbData(string orbID)
    {
        if (!orbTable.ContainsKey(orbID)) return null;

        return orbTable[orbID];
    }

    public override void Load(string name)
    {
        Addressables.LoadAssetAsync<TextAsset>(name).Completed += (textAsset) =>
        {
            using (var reader = new StringReader(textAsset.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csvReader.GetRecords<OrbData>();

                foreach (var record in records)
                {
                    orbTable.Add(record.Orb_ID.ToString(), record);
                }
            }
        };
    }
}
