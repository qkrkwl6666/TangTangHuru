using System.Collections.Generic;

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
    public static readonly string String = "String";
    public static readonly string stage = "Stage";

    private void Awake()
    {
        DataTable stageTable = new StageTable();
        stageTable.Load(stage);

        DataTable waveTable = new WaveTable();
        waveTable.Load(stageWave);

        DataTable monsterTable = new MonsterTable();
        monsterTable.Load(monster);

        DataTable monsterSkillTable = new MonsterSkillTable();
        monsterSkillTable.Load(monsterSkill);

        DataTable bossTable = new BossTable();
        bossTable.Load(boss);

        DataTable bossSkillTable = new BossSkillTable();
        bossSkillTable.Load(bossSkill);

        DataTable stageBossTable = new BossStageTable();
        stageBossTable.Load(stageBoss);

        DataTable treasureTable = new TreasureTable();
        treasureTable.Load(treasure);

        DataTable itemTable = new ItemTable();
        itemTable.Load(item);

        DataTable stringTable = new StringTable();
        stringTable.Load(String);

        tables.Add(stage, stageTable);

        tables.Add(stageWave, waveTable);
        tables.Add(monster, monsterTable);
        tables.Add(monsterSkill, monsterSkillTable);

        tables.Add(boss, bossTable);
        tables.Add(bossSkill, bossSkillTable);
        tables.Add(stageBoss, stageBossTable);

        tables.Add(treasure, treasureTable);
        tables.Add(item, itemTable);

        tables.Add(String, stringTable);
    }

    public void Update()
    {

    }

    public T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id)) return default;

        return tables[id] as T;
    }

}
