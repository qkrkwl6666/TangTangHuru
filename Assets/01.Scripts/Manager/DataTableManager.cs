using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class DataTableManager : MonoBehaviour
{
    private static Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();
    public static readonly string stageWave = "StageWave";
    public static readonly string monster = "Monster";
    public static readonly string monsterType = "MonsterType";

    private void Awake()
    {
        DataTable WaveTable = new WaveTable();
        WaveTable.Load(stageWave);

        DataTable monsterTable = new MonsterTable();
        monsterTable.Load(monster);

        DataTable monsterTypeTable = new MonsterTypeTable();
        monsterTypeTable.Load(monsterType);

        tables.Add(stageWave, WaveTable);
        tables.Add(monster, monsterTable);
        tables.Add(monsterType, monsterTypeTable);
    }

    public void Update()
    {
        
    }

    public static DataTable GetDataTable(string name)
    {
        if (tables.ContainsKey(name))
        {
            return tables[name];
        }

        return null;
    }

}
