using System;

/// <summary>
/// OrbUpgraderView 인터페이스
/// View는 UI 업데이트만 담당하고, 비즈니스 로직은 Presenter에서 처리
/// </summary>
public interface IOrbUpgraderView
{
    /// <summary>
    /// 슬롯 클릭 이벤트
    /// </summary>
    event Action<ItemSlotUI> OnSlotClicked;

    /// <summary>
    /// 업그레이드 버튼 클릭 이벤트
    /// </summary>
    event Action OnUpgradeButtonClicked;

    /// <summary>
    /// 모든 슬롯 초기화
    /// </summary>
    void ClearAllSlots();

    /// <summary>
    /// 오브 선택 패널 표시
    /// </summary>
    void ShowOrbPanel(ItemSlotUI targetSlot);

    /// <summary>
    /// 오브 선택 패널 숨김
    /// </summary>
    void HideOrbPanel();

    /// <summary>
    /// 오브 패널 갱신
    /// </summary>
    void RefreshOrbPanel();

    /// <summary>
    /// 업그레이드 완료 알림 패널 표시
    /// </summary>
    void ShowNoticePanel(ItemData itemData);

    /// <summary>
    /// 업그레이드 슬롯 배열 반환
    /// </summary>
    ItemSlotUI[] GetUpgradeSlots();
}
