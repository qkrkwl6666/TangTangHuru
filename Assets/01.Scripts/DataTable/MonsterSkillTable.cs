using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using CsvHelper;
using System.Globalization;
using System.IO;
using TextAsset = UnityEngine.TextAsset;

public class MonsterSkillData
{
    public int Skill_Id { get; set; }
    public string Skill_Prefab { get; set; }
    public float Skill_Rate { get; set; }
    public int Skill_Count { get; set; }
    public float Damage_Factor { get; set; }
}

public class MonsterSkillTable : DataTable
{
    public Dictionary<string, MonsterSkillData> monsterSkillTable { get; private set; } = new();
    public MonsterSkillData GetMonsterSkillData(string skillId)
    {
        if (!monsterSkillTable.ContainsKey(skillId)) return null;

        return monsterSkillTable[skillId];
    }
    public override void Load(string name)
    {
        Addressables.LoadAssetAsync<TextAsset>(name).Completed += (textAsset) =>
        {
            using (var reader = new StringReader(textAsset.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csvReader.GetRecords<MonsterSkillData>();

                foreach (var record in records)
                {
                    monsterSkillTable.Add(record.Skill_Id.ToString(), record);
                }
            }
        };
    }
}
