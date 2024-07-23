using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using CsvHelper;
using System.Globalization;
using System.IO;
using TextAsset = UnityEngine.TextAsset;

public class BossData
{
    public int Boss_Id { get; set; }
    public string Boss_Prefab { get; set; }
    public int Boss_Hp { get; set; }
    public float Boss_Damage { get; set; }
    public float Boss_MoveSpeed { get; set; }
    public float Boss_Cooldown { get; set; }

    public int Skill1_Id { get; set; }
    public float Skill1_Probability { get; set; }
    public int Skill2_Id { get; set; }
    public float Skill2_Probability { get; set; }
    public int Skill3_Id { get; set; }
    public float Skill3_Probability { get; set; }
    public int Skill4_Id { get; set; }
    public float Skill4_Probability { get; set; }
    public int Skill5_Id { get; set; }
    public float Skill5_Probability { get; set; }

    public List<(int, float)> GetBossSkillId()
    {
        List<(int, float)> skills = new ();

        skills.Add((Skill1_Id, Skill1_Probability));
        skills.Add((Skill2_Id, Skill2_Probability));
        skills.Add((Skill3_Id, Skill3_Probability));
        skills.Add((Skill4_Id, Skill4_Probability));
        skills.Add((Skill5_Id, Skill5_Probability));

        return skills;
    }
}

public class BossTable : DataTable
{
    public Dictionary<string, BossData> bossTable { get; private set; } = new();

    public BossData GetBossData(string bossId)
    {
        if (!bossTable.ContainsKey(bossId)) return null;

        return bossTable[bossId];
    }

    public override void Load(string name)
    {
        Addressables.LoadAssetAsync<TextAsset>(name).Completed += (textAsset) =>
        {
            using (var reader = new StringReader(textAsset.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csvReader.GetRecords<BossData>();

                foreach (var record in records)
                {
                    bossTable.Add(record.Boss_Id.ToString(), record);
                }
            }
        };
    }
}
