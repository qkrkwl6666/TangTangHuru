/// <summary>
/// OrbUpgrader 데이터 모델
/// 업그레이드 관련 상태 및 규칙을 관리
/// </summary>
public class OrbUpgraderModel
{
    // 상태 데이터
    public int FirstItemId { get; set; }
    public ItemData CurrentOrbData { get; set; }

    // 설정 데이터 (외부에서 주입)
    public int RequiredOrbCount { get; private set; }
    public int MaxUpgradeTier { get; private set; }

    public OrbUpgraderModel(int requiredOrbCount, int maxUpgradeTier)
    {
        FirstItemId = 0;
        CurrentOrbData = null;
        RequiredOrbCount = requiredOrbCount;
        MaxUpgradeTier = maxUpgradeTier;
    }

    /// 업그레이드 가능 여부
    public bool CanUpgrade(ItemSlotUI[] slots)
    {
        if (slots == null || slots.Length < RequiredOrbCount)
            return false;

        FirstItemId = slots[0].currItem?.ItemId ?? 0;
        if (FirstItemId == 0)
            return false;

        // 모든 슬롯이 동일한 아이템인지 확인
        for (int i = 1; i < RequiredOrbCount; i++)
        {
            if (slots[i].currItem == null || slots[i].currItem.ItemId != FirstItemId)
                return false;
        }

        return true;
    }

    public bool IsMaxTier(ItemData orbData)
    {
        return orbData.Item_Tier > MaxUpgradeTier;
    }
}
