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
    public static readonly string boss = "Boss";
    public static readonly string bossSkill = "BossSkill";

    private void Awake()
    {
        DataTable waveTable = new WaveTable();
        waveTable.Load(stageWave);

        DataTable monsterTable = new MonsterTable();
        monsterTable.Load(monster);

        DataTable monsterTypeTable = new MonsterTypeTable();
        monsterTypeTable.Load(monsterType);

        DataTable monsterSkillTable = new MonsterSkillTable();
        monsterSkillTable.Load(monsterSkill);

        // DataTable bossTable = new BossTable();
        // monsterSkillTable.Load(boss);
        
        DataTable bossSkillTable = new BossSkillTable();
        bossSkillTable.Load(bossSkill);

        tables.Add(stageWave, waveTable);
        tables.Add(monster, monsterTable);
        tables.Add(monsterType, monsterTypeTable);
        tables.Add(monsterSkill, monsterSkillTable);
        //tables.Add(boss, bossTable);
        tables.Add(bossSkill, bossSkillTable);
    }

    public T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id)) return default;

        return tables[id] as T;
    }

}
