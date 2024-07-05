using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    public int stage;

    public List<WaveData> waveDatas;
    private WaveData currentWaveData = null;
    private int waveIndex = 0;

    private float totalWaveTime;
    private float wave1Time;
    private float wave2Time;
    private float wave3Time;

    MonsterSpawnFactory monsterSpawnFactory = null;

    private void Awake()
    {
        waveDatas = (DataTableManager.GetDataTable(DataTableManager.stageWave) 
            as WaveTable).waveTable[stage.ToString()];

        currentWaveData = waveDatas[waveIndex++];

        monsterSpawnFactory = GetComponent<MonsterSpawnFactory>();
    }

    public void Update()
    {
        totalWaveTime += Time.deltaTime;
        wave1Time += Time.deltaTime;
        wave2Time += Time.deltaTime;
        wave3Time += Time.deltaTime;

        if (totalWaveTime >= currentWaveData.duration)
        {
            currentWaveData = waveDatas[waveIndex++];
            totalWaveTime = 0f;
        }

        if(wave1Time >= currentWaveData.monster1_Duration && currentWaveData.monster1_Duration != -1)
        {
            wave1Time = 0f;
            monsterSpawnFactory.CreateMonster((MonsterType)currentWaveData.monster1_Id, 
                currentWaveData.monster1_Count, currentWaveData.spawn1_Type);
        }
        if (wave2Time >= currentWaveData.monster2_Duration && currentWaveData.monster2_Duration != -1)
        {
            wave2Time = 0f;
            monsterSpawnFactory.CreateMonster((MonsterType)currentWaveData.monster2_Id,
                currentWaveData.monster2_Count, currentWaveData.spawn2_Type);
        }
        if (wave3Time >= currentWaveData.monster3_Duration && currentWaveData.monster3_Duration != -1)
        {
            wave3Time = 0f;
            monsterSpawnFactory.CreateMonster((MonsterType)currentWaveData.monster3_Id,
                currentWaveData.monster3_Count, currentWaveData.spawn3_Type);
        }
    }
 
}
