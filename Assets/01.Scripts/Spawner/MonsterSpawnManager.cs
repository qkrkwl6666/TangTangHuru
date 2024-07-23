using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    public List<WaveData> waveDatas;
    private WaveData currentWaveData = null;
    private int waveIndex = 0;
    public bool IsStop { get; private set; }
    public Action OnStop;

    private Dictionary<int, MonsterSpawnInfo> monsterSpawnInfos = new Dictionary<int, MonsterSpawnInfo>();

    private float totalWaveTime;

    private MonsterSpawnFactory monsterSpawnFactory = null;

    private void Awake()
    {
        waveDatas = DataTableManager.Instance.Get<WaveTable>(DataTableManager.stageWave).
            waveTable[GameManager.Instance.CurrentStage.ToString()];

        monsterSpawnFactory = GetComponent<MonsterSpawnFactory>();

        InitializeSpawnInfos();

        NextWave();

        OnStop += () => 
        {
            IsStop = true;
            SpawnBoss();
        };
    }

    public void Update()
    {
        totalWaveTime += Time.deltaTime;

        if(IsStop) return;

        if (totalWaveTime >= currentWaveData.duration)
        {
            NextWave();
        }

        foreach(var spawnData in monsterSpawnInfos.Values)
        {
            spawnData.Update(Time.deltaTime);
            if(spawnData.IsSpawn)
            {
                SpawnMonster(spawnData);
            }
        }
    }

    public void SpawnMonster(MonsterSpawnInfo monsterSpawnInfo)
    {
        if(monsterSpawnInfo.IsValid)
        {
            monsterSpawnFactory.CreateMonster(DataTableManager.Instance.Get<MonsterTable>
                (DataTableManager.monster).GetMonsterData(monsterSpawnInfo.MonsterId.ToString()), 
                monsterSpawnInfo.MonsterCount, monsterSpawnInfo.SpawnType);
        }
    }

    private void InitializeSpawnInfos()
    {
        monsterSpawnInfos[1] = new MonsterSpawnInfo();
        monsterSpawnInfos[2] = new MonsterSpawnInfo();
        monsterSpawnInfos[3] = new MonsterSpawnInfo();
    }

    private void UpdateSpawnInfos()
    {
        monsterSpawnInfos[1].SetSpawnData(currentWaveData.monster1_Id, currentWaveData.monster1_Count,
            currentWaveData.monster1_Duration, currentWaveData.spawn1_Type);
        monsterSpawnInfos[2].SetSpawnData(currentWaveData.monster2_Id, currentWaveData.monster2_Count,
            currentWaveData.monster2_Duration, currentWaveData.spawn2_Type);
        monsterSpawnInfos[3].SetSpawnData(currentWaveData.monster3_Id, currentWaveData.monster3_Count,
            currentWaveData.monster3_Duration, currentWaveData.spawn3_Type);
    }

    public void NextWave()
    {
        if (waveIndex < waveDatas.Count)
        {
            totalWaveTime = 0f;
            currentWaveData = waveDatas[waveIndex];
            UpdateSpawnInfos();
            waveIndex++;
        }
        else
        {
            Debug.Log("All Waves Completed");
            enabled = false;
        }
    }

    public void SpawnBoss()
    {
        monsterSpawnFactory.MonsterAllDead();

        var bossStageData = DataTableManager.Instance.Get<BossStageTable>
            (DataTableManager.stageBoss).GetBossData(GameManager.Instance.CurrentStage.ToString());

        var bossData = DataTableManager.Instance.Get<BossTable>(DataTableManager.boss)
            .GetBossData(bossStageData.Boss_Id.ToString());

        monsterSpawnFactory.CreateBoss(bossData);
    }


}
