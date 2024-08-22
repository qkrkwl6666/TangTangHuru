using System.Collections.Generic;

public abstract class SaveDatas
{
    public int Version { get; protected set; }
    public abstract SaveDatas VersionUp();
}

public class SaveDataV1 : SaveDatas
{
    public int Gold { get; set; } = 1000000;
    public int Diamond { get; set; } = 0;
    public int CurrentStage { get; set; } = 1;
    public int MaxStage { get; set; } = 1;

    public bool isTutorialCompleted = false;

    // ��ü ������ �����̳�
    public List<Item> allItem = new();

    // �÷��̾ �������ִ� ������ �����̳� ��� 
    public Dictionary<PlayerEquipment, Item> playerEquipment = new();

    // �������� �������� ����
    public List<AchieveState> SavedStates = new();
    public List<int> SavedAchieveProgress = new();
    // �������� �������� ����
    public List<bool> scrollStates = new();
    public List<bool> petRewardStates = new();


    //��ȭ�� ������
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