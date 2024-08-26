using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public List<string> armorSetNames = new List<string>
    {
        "HolyKnightHelmet",
        "HolyKnightArmor",
        "HolyKnightShoes",

        "SilverStriderHelmet",
        "SilverStriderArmor",
        "SilverStriderShoes",

        "ShadowWorkHelmet",
        "ShadowWorkArmor",
        "ShadowWorkShoes",

        "RedStoneHelmet",
        "RedStoneArmor",
        "RedStoneShoes",

        "StormBreakerHelmet",
        "StormBreakerArmor",
        "StormBreakerShoes",

        "MoonWalkerHelmet",
        "MoonWalkerArmor",
        "MoonWalkerShoes",

        "SkyWatchHelmet",
        "SkyWatchArmor",
        "SkyWatchShoes",
    };

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
            { "AppraisalCount", 0 },

            {"HolyKnightHelmet", 0 },
            {"HolyKnightArmor", 0 },
            {"HolyKnightShoes", 0 },

            {"SilverStriderHelmet", 0 },
            {"SilverStriderArmor", 0 },
            {"SilverStriderShoes", 0 },

            {"ShadowWorkHelmet", 0 },
            {"ShadowWorkArmor", 0 },
            {"ShadowWorkShoes", 0 },

            {"RedStoneHelmet", 0 },
            {"RedStoneArmor", 0 },
            {"RedStoneShoes", 0 },

            {"StormBreakerHelmet", 0 },
            {"StormBreakerArmor", 0 },
            {"StormBreakerShoes", 0 },

            {"MoonWalkerHelmet", 0 },
            {"MoonWalkerArmor", 0 },
            {"MoonWalkerShoes", 0 },

            {"SkyWatchHelmet", 0 },
            {"SkyWatchArmor", 0 },
            {"SkyWatchShoes", 0 },

        };

    public Dictionary<string, int> completeConditions
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
            { "AppraisalCount", 20 },

            {"HolyKnightHelmet", 1 },
            {"HolyKnightArmor", 1 },
            {"HolyKnightShoes", 1 },

            {"SilverStriderHelmet", 1 },
            {"SilverStriderArmor", 1 },
            {"SilverStriderShoes", 1 },

            {"ShadowWorkHelmet", 1 },
            {"ShadowWorkArmor", 1 },
            {"ShadowWorkShoes", 1 },

            {"RedStoneHelmet", 1 },
            {"RedStoneArmor", 1 },
            {"RedStoneShoes", 1 },

            {"StormBreakerHelmet", 1 },
            {"StormBreakerArmor", 1 },
            {"StormBreakerShoes", 1 },

            {"MoonWalkerHelmet", 1 },
            {"MoonWalkerArmor", 1 },
            {"MoonWalkerShoes", 1 },

            {"SkyWatchHelmet", 1 },
            {"SkyWatchArmor", 1 },
            {"SkyWatchShoes", 1 },
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

    public void AddProgress(string key, int num) //해당 도전과제 진행도 증가
    {
        if (progressValues.ContainsKey(key))
        {
            progressValues[key] += num;
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
        foreach (var key in progressValues.Keys)
        {
            SavedAchieveProgress.Add(progressValues[key]);
        }
    }

    public void LoadProgress()
    {
        for (int i = 0; i < progressValues.Keys.Count; i++)
        {
            progressValues[progressValues.Keys.ElementAt(i)] = SavedAchieveProgress[i];
        }
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
        myTasks = new AhievementTask();

        var stringTable = DataTableManager.Instance.Get<StringTable>(DataTableManager.String);

        List<string> keyList = new List<string>(myTasks.completeConditions.Keys);

        for (int i = 0; i < 12; i++)
        {
            achievements.Add(new Achievement
            {
                title = keyList[i],
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

    //도전과제 달성확인
    public bool Check(string taskName)
    {
        return myTasks.CheckCompleted(taskName);
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

        for (int i = 0; i < achievements.Count; i++)
        {
            SavedStates.Add(achievements[i].achieveState);
        }
    }

    public List<string> GetArmorNameList()
    {
        return myTasks.armorSetNames;
    }
}
