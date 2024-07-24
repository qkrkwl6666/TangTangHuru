using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine.AddressableAssets;
using TextAsset = UnityEngine.TextAsset;

public class MonsterData
{
    public int Monster_ID { get; set; }
    public int Monster_Prefab { get; set; }
    public float Monster_Scale { get; set; }
    public float Monster_Hp { get; set; }
    public float Monster_MoveSpeed { get; set; }
    public float Monster_Damage { get; set; }
    public float Monster_Exp { get; set; }
    public int Monster_Gold { get; set; }
    public int Monster_Skill_Id { get; set; }
    public float Cooldown { get; set; }
    public float Range { get; set; }
}

public class MonsterTable : DataTable
{
    public Dictionary<string, MonsterData> monsterTable { get; private set; } = new();

    public MonsterData GetMonsterData(string monsterID)
    {
        if (!monsterTable.ContainsKey(monsterID)) return null;

        return monsterTable[monsterID];
    }

    public override void Load(string name)
    {
        Addressables.LoadAssetAsync<TextAsset>(name).Completed += (textAsset) =>
        {
            using (var reader = new StringReader(textAsset.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csvReader.GetRecords<MonsterData>();

                foreach (var record in records)
                {
                    monsterTable.Add(record.Monster_ID.ToString(), record);
                }
            }
        };
    }
}
