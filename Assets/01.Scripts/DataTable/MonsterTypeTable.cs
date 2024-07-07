using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using CsvHelper;
using System.Globalization;
using System.IO;

public class MonsterTypeData
{
    public int Type_Id {  get; set; }
    public int Skill1_Id {  get; set; }
    public int Skill2_Id {  get; set; }
    public int Skill3_Id {  get; set; }
    public int Skill4_Id {  get; set; }
    public int Skill5_Id {  get; set; }
}

public class MonsterTypeTable : DataTable
{
    public Dictionary<string, MonsterTypeData> monsterTypeTable { get; private set; } = new();

    public override void Load(string name)
    {
        Addressables.LoadAssetAsync<TextAsset>(name).Completed += (textAsset) =>
        {
            using (var reader = new StringReader(textAsset.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csvReader.GetRecords<MonsterTypeData>();

                foreach (var record in records)
                {
                    monsterTypeTable.Add(record.Type_Id.ToString(), record);
                }
            }
        };
    }
}
