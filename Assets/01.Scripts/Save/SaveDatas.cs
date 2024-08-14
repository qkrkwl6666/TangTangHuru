using System.Collections.Generic;

public abstract class SaveDatas 
{
    public int Version { get; protected set; }
    public abstract SaveDatas VersionUp();
}

public class SaveDataV1 : SaveDatas
{
    public int Gold { get; set; } = 100000;
    public int Diamond { get; set; } = 0;
    public int CurrentStage { get; set; } = 1;

    // 전체 아이템 컨테이너
    public List<Item> allItem = new();

    // 플레이어가 가지고있는 아이템 컨테이너 장비 
    public Dictionary<PlayerEquipment, Item> playerEquipment = new ();

    // 도전과제 진행정보 저장
    public List<AchieveState> SavedStates = new();
    public List<int> SavedAchieveProgress = new();

    //강화석 게이지
    public int gaige = 0;

    public SaveDataV1()
    {
        Version = 1;
    }

    public override SaveDatas VersionUp()
    {
        //SaveDataV2 saveData = new SaveDataV2();
        //saveData.Gold = Gold;

        return null;
    }
}