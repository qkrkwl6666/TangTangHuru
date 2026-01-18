using System;

/// <summary>
/// OrbCrafterView 인터페이스
/// View는 UI 업데이트만 담당하고, 비즈니스 로직은 Presenter에서 처리
/// </summary>
public interface IOrbCrafterView
{
    /// <summary>
    /// 제작 버튼 클릭 이벤트
    /// </summary>
    event Action OnCraftButtonClicked;

    /// <summary>
    /// 강화석 개수 표시 업데이트
    /// </summary>
    void SetStoneCount(int count);

    /// <summary>
    /// 성공 확률 표시 업데이트
    /// </summary>
    void SetSuccessPercent(int percent);

    /// <summary>
    /// 게이지 UI 업데이트
    /// </summary>
    void UpdateGaige(int filledCount, int maxCount);

    /// <summary>
    /// 게이지 초기화 (모두 투명하게)
    /// </summary>
    void ResetGaige();

    /// <summary>
    /// 오브 획득 알림 패널 표시
    /// </summary>
    void ShowNoticePanel(ItemData itemData);

    /// <summary>
    /// OrbPanel 갱신
    /// </summary>
    void RefreshOrbPanel();
}
