using System.Collections.Generic;

/// <summary>
/// 게이지, 강화석 개수, 제작 확률 등의 상태를 관리
/// </summary>
public class OrbCrafterModel
{
    // 상태 데이터
    public int GaigeNum { get; set; }
    public int StoneCount { get; set; }
    public int CreatePercent { get; set; }

    // 설정 데이터 (외부에서 주입)
    public int MaxGaige { get; private set; }
    public int StonesPerCraft { get; private set; }
    public List<int> OrbIdList { get; private set; }

    /// <summary>
    /// 설정값을 외부에서 주입받는 생성자
    /// </summary>
    public OrbCrafterModel(int maxGaige, int stonesPerCraft, int createPercent, List<int> orbIdList)
    {
        GaigeNum = 0;
        StoneCount = 0;
        CreatePercent = createPercent;
        MaxGaige = maxGaige;
        StonesPerCraft = stonesPerCraft;
        OrbIdList = new List<int>(orbIdList); // 복사본 생성
    }

    public bool CanCraft()
    {
        return StoneCount >= StonesPerCraft;
    }

    public bool ShouldIncrementGaige()
    {
        return GaigeNum < MaxGaige;
    }

    public void ResetGaige()
    {
        GaigeNum = 0;
    }

    public int GetRandomOrbId()
    {
        return OrbIdList[UnityEngine.Random.Range(0, OrbIdList.Count)];
    }

    public void IncreasePercent(int amount)
    {
        CreatePercent += amount;
    }
}
