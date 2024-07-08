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

    public List<int> GetSkillDatas()
    {
        var list = new List<int>();

        list.Add(Skill1_Id);
        list.Add(Skill2_Id);
        list.Add(Skill3_Id);
        list.Add(Skill4_Id);
        list.Add(Skill5_Id);

        return list;
    }
}

public class MonsterTypeTable : DataTable
{
    public Dictionary<string, MonsterTypeData> monsterTypeTable { get; private set; } = new();

    public MonsterTypeData GetMonsterTypeData(string monsterType)
    {
        if (!monsterTypeTable.ContainsKey(monsterType)) return null;

        return monsterTypeTable[monsterType];
    }

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
