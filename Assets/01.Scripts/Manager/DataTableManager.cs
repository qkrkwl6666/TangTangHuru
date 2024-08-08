using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using UnityEngine;

public class DataTableManager : Singleton<DataTableManager>
{
    private static Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();

    public static readonly string stageWave = "StageWave";
    public static readonly string monster = "Monster";
    public static readonly string monsterSkill = "MonsterSkill";
    public static readonly string boss = "Boss";
    public static readonly string bossSkill = "BossSkill";
    public static readonly string stageBoss = "StageBoss";
    public static readonly string treasure = "Treasure";
    public static readonly string item = "Item"; 
    public static readonly string orb = "Orb"; 
    public static readonly string String = "String";
    public static readonly string stage = "Stage";

    public static readonly string[] TableNames = 
    {
        "StageWave", "Monster", "MonsterSkill", "Boss", "BossSkill", "StageBoss",
        "Treasure", "Item", "Orb", "String", "Stage"
    };

    public event Action OnTableLoaded;

    public Action OnAllTableLoaded;

    public bool isTableLoad = false;

    private int loadTableCount = 0;

    public int tableCount = 0;

    private void Awake()
    {
        tableCount = TableNames.Length;

        OnTableLoaded += TableLoadCompleted;

        DataTable stageTable = new StageTable();
        stageTable.Load(stage, OnTableLoaded);

        DataTable waveTable = new WaveTable();
        waveTable.Load(stageWave, OnTableLoaded);

        DataTable monsterTable = new MonsterTable();
        monsterTable.Load(monster, OnTableLoaded);

        DataTable monsterSkillTable = new MonsterSkillTable();
        monsterSkillTable.Load(monsterSkill, OnTableLoaded);

        DataTable bossTable = new BossTable();
        bossTable.Load(boss, OnTableLoaded);

        DataTable bossSkillTable = new BossSkillTable();
        bossSkillTable.Load(bossSkill, OnTableLoaded);

        DataTable stageBossTable = new BossStageTable();
        stageBossTable.Load(stageBoss, OnTableLoaded);

        DataTable treasureTable = new TreasureTable();
        treasureTable.Load(treasure, OnTableLoaded);

        DataTable itemTable = new ItemTable();
        itemTable.Load(item, OnTableLoaded);

        DataTable orbTable = new OrbTable();
        orbTable.Load(orb, OnTableLoaded);

        DataTable stringTable = new StringTable();
        stringTable.Load(String, OnTableLoaded);

        tables.Add(stage, stageTable);

        tables.Add(stageWave, waveTable);
        tables.Add(monster, monsterTable);
        tables.Add(monsterSkill, monsterSkillTable);

        tables.Add(boss, bossTable);
        tables.Add(bossSkill, bossSkillTable);
        tables.Add(stageBoss, stageBossTable);

        tables.Add(treasure, treasureTable);
        tables.Add(item, itemTable);
        tables.Add(orb, orbTable);

        tables.Add(String, stringTable);
    }

    public void TableLoadCompleted()
    {
        UnityEngine.Debug.Log("TableLoadCompleted");

        loadTableCount++;

        //UnityEngine.Debug.Log(loadTableCount);

        if (loadTableCount >= tables.Count) 
        {
            OnAllTableLoaded?.Invoke();
            isTableLoad = true;
        }
    }

    private DataTable CreateTableInstance(string tableName)
    {
        switch (tableName)
        {
            case "Stage": return new StageTable();
            case "StageWave": return new WaveTable();
            case "Monster": return new MonsterTable();
            case "MonsterSkill": return new MonsterSkillTable();
            case "Boss": return new BossTable();
            case "BossSkill": return new BossSkillTable();
            case "StageBoss": return new BossStageTable();
            case "Treasure": return new TreasureTable();
            case "Item": return new ItemTable();
            case "Orb": return new OrbTable();
            case "String": return new StringTable();

            default: throw new ArgumentException($"Unknown table type: {tableName}");
        }
    }

    public T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id)) return default;

        return tables[id] as T;
    }

}
