using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class DataTableManager : Singleton<DataTableManager>
{
    private static Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();
    public static readonly string stageWave = "StageWave";
    public static readonly string monster = "Monster";
    public static readonly string monsterType = "MonsterType";
    public static readonly string monsterSkill = "MonsterSkill";

    private void Awake()
    {
        DataTable WaveTable = new WaveTable();
        WaveTable.Load(stageWave);

        DataTable monsterTable = new MonsterTable();
        monsterTable.Load(monster);

        DataTable monsterTypeTable = new MonsterTypeTable();
        monsterTypeTable.Load(monsterType);

        DataTable monsterSkillTable = new MonsterSkillTable();
        monsterSkillTable.Load(monsterSkill);

        tables.Add(stageWave, WaveTable);
        tables.Add(monster, monsterTable);
        tables.Add(monsterType, monsterTypeTable);
        tables.Add(monsterSkill, monsterSkillTable);
    }

    public T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id)) return default;

        return tables[id] as T;
    }

}
