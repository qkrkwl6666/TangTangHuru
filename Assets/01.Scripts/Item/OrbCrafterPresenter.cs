using UnityEngine;

/// <summary>
/// 모든 비즈니스 로직을 처리, View 인터페이스를 통해서만 UI를 조작
/// </summary>
public class OrbCrafterPresenter
{
    private readonly IOrbCrafterView view;
    private readonly OrbCrafterModel model;
    private readonly IInventoryService inventory;

    public OrbCrafterPresenter(IOrbCrafterView view, OrbCrafterModel model, IInventoryService inventory)
    {
        this.view = view;
        this.model = model;
        this.inventory = inventory;

        // View 이벤트 구독
        view.OnCraftButtonClicked += HandleCraft;
    }

    public void Initialize()
    {
        LoadGaige();
        view.ResetGaige();
        UpdateStoneCount();
        view.SetSuccessPercent(model.CreatePercent);
        view.UpdateGaige(model.GaigeNum, model.MaxGaige);
    }

    public void OnViewEnabled()
    {
        UpdateStoneCount();
    }

    private void HandleCraft()
    {
        if (!model.CanCraft())
        {
            return;
        }

        // 강화석 소모
        inventory.RemoveItem(ItemType.ReinforcedStone, ItemTier.Normal, model.StonesPerCraft);

        // 성공 여부 판정
        bool success = Random.Range(0, 100) < model.CreatePercent;

        if (success)
        {
            HandleCraftSuccess();
        }
        else
        {
            HandleCraftFail();
        }

        // UI 갱신
        inventory.RefreshItemSlotUI();
        view.RefreshOrbPanel();
        UpdateStoneCount();
    }

    /// <summary>
    /// 제작 성공 처리
    /// </summary>
    private void HandleCraftSuccess()
    {
        if (model.ShouldIncrementGaige())
        {
            // 게이지 증가
            model.GaigeNum++;
            view.UpdateGaige(model.GaigeNum, model.MaxGaige);
            SoundManager.Instance.PlaySound2D("success");
        }
        else
        {
            // 게이지 가득참 - 오브 생성
            model.ResetGaige();
            view.ResetGaige();

            int orbId = model.GetRandomOrbId();
            inventory.MainInventoryAddItem(orbId.ToString());

            var orbData = DataTableManager.Instance
                .Get<ItemTable>(DataTableManager.item)
                .GetItemData(orbId.ToString());

            view.ShowNoticePanel(orbData);
            SoundManager.Instance.PlaySound2D("orb");
        }

        SaveGaige();
    }

    /// <summary>
    /// 제작 실패 처리
    /// </summary>
    private void HandleCraftFail()
    {
        SoundManager.Instance.PlaySound2D("failed");
    }

    /// <summary>
    /// 강화석 개수 업데이트
    /// </summary>
    private void UpdateStoneCount()
    {
        model.StoneCount = inventory.GetItemCount(ItemType.ReinforcedStone, ItemTier.Normal);
        view.SetStoneCount(model.StoneCount);
    }

    /// <summary>
    /// 게이지 저장
    /// </summary>
    private void SaveGaige()
    {
        SaveManager.SaveDataV1.gaige = model.GaigeNum;
    }

    /// <summary>
    /// 게이지 로드
    /// </summary>
    private void LoadGaige()
    {
        model.GaigeNum = SaveManager.SaveDataV1.gaige;
    }

    /// <summary>
    /// 성공 확률 증가 (외부 호출용)
    /// </summary>
    public void IncreasePercent(int amount)
    {
        model.IncreasePercent(amount);
        view.SetSuccessPercent(model.CreatePercent);
    }

    /// <summary>
    /// 리소스 정리
    /// </summary>
    public void Dispose()
    {
        view.OnCraftButtonClicked -= HandleCraft;
    }
}
