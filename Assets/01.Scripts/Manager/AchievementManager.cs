using System;
using System.Collections.Generic;
using UnityEngine;

public enum AchieveState
{
    Incompleted,
    Completed,
    Rewarded,
}

[Serializable]
public class Achievement
{
    //저장정보
    public AchieveState achieveState;

    public string title;
    public string description;

    public Action onUnlock;

    public void Unlock()
    {
        if (achieveState == AchieveState.Incompleted)
        {
            achieveState = AchieveState.Completed;
            Debug.Log($"Achievement Unlocked: {title}");
            onUnlock?.Invoke();  // 도전과제 달성 시 추가 작업
        }
    }
}

public class AhievementTask
{
    public List<int> SavedAchieveProgress = new List<int>();

    private Dictionary<string, int> progressValues
     = new Dictionary<string, int>
        {
            { "SummonedCount", 0 },
            { "KilledCount", 0 },
            { "CoreCount", 0 },
            { "WallUsedCount", 0 },
            { "HealthPercent", 0 },
            { "WallCount", 0 },
            { "AcendCount", 0 },
            { "UsedGold", 0 },
            { "TotalPurchase", 0 },
            { "ReinforcedArmorCount", 0 },
            { "ReinforcedWeaponCount", 0 },
            { "EpicCount", 0 },
            { "AppraisalCount", 0 }
        };

    private Dictionary<string, int> completeConditions
    = new Dictionary<string, int>
    {
            { "SummonedCount", 2 },
            { "KilledCount", 3 },
            { "CoreCount", 3 },
            { "WallUsedCount", 0 },
            { "HealthPercent", 70 },
            { "WallCount", 3 },
            { "AcendCount", 5 },
            { "UsedGold", 100000 },
            { "TotalPurchase", 3 },
            { "ReinforcedArmorCount", 10 },
            { "ReinforcedWeaponCount", 5 },
            { "EpicCount", 5 },
            { "AppraisalCount", 20 }
    };

    public void AddProgress(string key) //해당 도전과제 진행도 증가
    {
        if (progressValues.ContainsKey(key))
        {
            progressValues[key] += 1;
        }
        else
        {
            Debug.LogError("Invalid key value!");
        }
    }

    public bool CheckCompleted(string key)
    {
        if (progressValues.ContainsKey(key))
        {
            return (progressValues[key] >= completeConditions[key]);
        }
        else
        {
            Debug.LogError("Invalid key value!");
            return false;
        }
    }

    public int GetProgress(string key) //현재 진행도 확인
    {
        if (progressValues.ContainsKey(key))
        {
            return progressValues[key];
        }
        else
        {
            Debug.LogError("Invalid key value!");
            return -1;
        }
    }
    public int GetCondition(string key) //달성 조건 확인
    {
        if (completeConditions.ContainsKey(key))
        {
            return completeConditions[key];
        }
        else
        {
            Debug.LogError("Invalid key value!");
            return -1;
        }
    }

    public void SaveProgress()
    {
        SavedAchieveProgress.Add(progressValues["SummonedCount"]);
        SavedAchieveProgress.Add(progressValues["KilledCount"]);
        SavedAchieveProgress.Add(progressValues["CoreCount"]);
        SavedAchieveProgress.Add(progressValues["WallUsedCount"]);
        SavedAchieveProgress.Add(progressValues["HealthPercent"]);
        SavedAchieveProgress.Add(progressValues["WallCount"]);
        SavedAchieveProgress.Add(progressValues["AcendCount"]);
        SavedAchieveProgress.Add(progressValues["UsedGold"]);
        SavedAchieveProgress.Add(progressValues["TotalPurchase"]);
        SavedAchieveProgress.Add(progressValues["ReinforcedArmorCount"]);
        SavedAchieveProgress.Add(progressValues["ReinforcedWeaponCount"]);
        SavedAchieveProgress.Add(progressValues["EpicCount"]);
        SavedAchieveProgress.Add(progressValues["AppraisalCount"]);
    }

    public void LoadProgress()
    {
        progressValues["SummonedCount"] = SavedAchieveProgress[0];
        progressValues["KilledCount"] = SavedAchieveProgress[1];
        progressValues["CoreCount"] = SavedAchieveProgress[2];
        progressValues["WallUsedCount"] = SavedAchieveProgress[3];
        progressValues["HealthPercent"] = SavedAchieveProgress[4];
        progressValues["WallCount"] = SavedAchieveProgress[5];
        progressValues["AcendCount"] = SavedAchieveProgress[6];
        progressValues["UsedGold"] = SavedAchieveProgress[7];
        progressValues["TotalPurchase"] = SavedAchieveProgress[8];
        progressValues["ReinforcedArmorCount"] = SavedAchieveProgress[9];
        progressValues["ReinforcedWeaponCount"] = SavedAchieveProgress[10];
        progressValues["EpicCount"] = SavedAchieveProgress[11];
        progressValues["AppraisalCount"] = SavedAchieveProgress[12];
    }
}

public class AchievementManager : Singleton<AchievementManager>
{
    public List<AchieveState> SavedStates = new List<AchieveState>();

    private int completeCount = 0;
    public int CompletedCount { get { return completeCount; } }

    public AhievementTask myTasks;

    // 도전과제 리스트
    public List<Achievement> achievements = new List<Achievement>();

    // 도전과제 초기화 및 등록
    private void Start()
    {
        InitializeAchievements();
    }

    // 도전과제 초기화 메소드
    private void InitializeAchievements()
    {
        achievements.Clear();

        var stringTable = DataTableManager.Instance.Get<StringTable>(DataTableManager.String);

        for (int i = 0; i < 12; i++)
        {
            achievements.Add(new Achievement
            {
                title = stringTable.Get($"Achieve_Name{i + 1}").Text,
                description = stringTable.Get($"Achieve_Desc{i + 1}").Text,
                onUnlock = () => Debug.Log("Achievement Unlocked!")
            });
        }


        if (SaveManager.SaveDataV1.SavedAchieveProgress.Count == 0)
            return;
        myTasks.SavedAchieveProgress = SaveManager.SaveDataV1.SavedAchieveProgress;
        myTasks.LoadProgress();

        if (SaveManager.SaveDataV1.SavedStates.Count == 0)
            return;
        for (int i = 0; i < 12; i++)
        {
            achievements[i].achieveState = SaveManager.SaveDataV1.SavedStates[i];
        }

    }

    //도전과제 조건확인
    public void Check(string taskName)
    {
        myTasks.CheckCompleted(taskName);
    }

    //도전과제 달성
    public void UnlockAchievement(string achievementTitle)
    {
        var achievement = achievements.Find(a => a.title == achievementTitle);
        if (achievement != null && achievement.achieveState == AchieveState.Incompleted)
        {
            achievement.Unlock();
            completeCount++;
        }
    }


    public void SaveAchievement()
    {
        SavedStates.Clear();

        myTasks.SaveProgress();

        for(int i = 0; i < achievements.Count; i++)
        {
            SavedStates.Add(achievements[i].achieveState);
        }
    }
}
