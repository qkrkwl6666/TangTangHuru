using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class DataTableManager : MonoBehaviour
{
    private Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();
    public static readonly string stageWave = "StageWave";

    private void Awake()
    {
        DataTable WaveTable = new WaveTable();
        WaveTable.Load(stageWave);  

        tables.Add(stageWave, WaveTable);
    }

    private void Update()
    {
        //Debug.Log("Hello");
    }
}
