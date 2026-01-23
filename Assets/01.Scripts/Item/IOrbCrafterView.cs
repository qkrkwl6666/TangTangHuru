using System;

/// <summary>
/// OrbCrafterView 인터페이스
/// View는 UI 업데이트만 담당하고, 비즈니스 로직은 Presenter에서 처리
/// </summary>
public interface IOrbCrafterView
{
    // 제작 버튼 클릭 이벤트
    event Action OnCraftButtonClicked;

    // 강화석 개수 표시 업데이트
    void SetStoneCount(int count);

    // 성공 확률 표시 업데이트
    void SetSuccessPercent(int percent);

    // 게이지 UI 업데이트
    void UpdateGaige(int filledCount, int maxCount);

    // 게이지 초기화 (모두 투명하게)
    void ResetGaige();

    // 오브 획득 알림 패널 표시
    void ShowNoticePanel(ItemData itemData);

    // OrbPanel 갱신
    void RefreshOrbPanel();
}
