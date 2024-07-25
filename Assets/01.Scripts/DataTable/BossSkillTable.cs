using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine.AddressableAssets;
using TextAsset = UnityEngine.TextAsset;

public class BossSkillData
{
    public int BossSkill_Id { get; set; }
    public string Preafab_Id { get; set; }
    public float Damage_Factor { get; set; }
    public int Skill_Count { get; set; }
    public float Skill_Rate { get; set; }
}
public class BossSkillTable : DataTable
{
    public Dictionary<string, BossSkillData> bossSkillTable { get; private set; } = new();

    public BossSkillData Get(string name)
    {
        if (!bossSkillTable.ContainsKey(name)) return null;

        return bossSkillTable[name];
    }

    public override void Load(string name)
    {
        Addressables.LoadAssetAsync<TextAsset>(name).Completed += (textAsset) =>
        {
            using (var reader = new StringReader(textAsset.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csvReader.GetRecords<BossSkillData>();

                foreach (var record in records)
                {
                    bossSkillTable.Add(record.BossSkill_Id.ToString(), record);
                }
            }
        };
    }
}
