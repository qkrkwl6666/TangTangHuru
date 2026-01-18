using System.Collections.Generic;

/// <summary>
/// 업그레이드 로직을 처리, View 인터페이스를 통해서만 UI를 조작
/// </summary>
public class OrbUpgraderPresenter
{
    private readonly IOrbUpgraderView view;
    private readonly OrbUpgraderModel model;
    private readonly IInventoryService inventory;

    public OrbUpgraderPresenter(IOrbUpgraderView view, OrbUpgraderModel model, IInventoryService inventory)
    {
        this.view = view;
        this.model = model;
        this.inventory = inventory;

        // View 이벤트 구독
        view.OnSlotClicked += HandleSlotClicked;
        view.OnUpgradeButtonClicked += HandleUpgrade;
    }

    public void Initialize()
    {
        // 필요시 추가 초기화
    }

    private void HandleSlotClicked(ItemSlotUI slot)
    {
        if (slot.isSelected)
        {
            UndoSelect(slot);
        }
        else
        {
            view.ShowOrbPanel(slot);
        }

        SoundManager.Instance.PlaySound2D("cancel");
    }

    private void UndoSelect(ItemSlotUI slot)
    {
        slot.ClearInfo();
    }

    public void SelectOrbInPanel(int index, List<OrbDesc> orbList, ItemSlotUI currentSlot)
    {
        if (orbList == null || index < 0 || index >= orbList.Count)
            return;

        currentSlot.SetOrbInfo(orbList[index]);
        currentSlot.AddOrbInfo();
        view.HideOrbPanel();
    }
    private void HandleUpgrade()
    {
        var slots = view.GetUpgradeSlots();

        if (!model.CanUpgrade(slots))
        {
            SoundManager.Instance.PlaySound2D("failed");
            return;
        }

        // 오브 데이터 조회
        var orbData = DataTableManager.Instance
            .Get<ItemTable>(DataTableManager.item)
            .GetItemData(model.FirstItemId.ToString());

        // 최대 티어 검증
        if (model.IsMaxTier(orbData))
        {
            SoundManager.Instance.PlaySound2D("failed");
            return;
        }

        // 업그레이드 수행
        inventory.RemoveOrbItem(
            (ItemType)orbData.Item_Type,
            (ItemTier)orbData.Item_Tier,
            model.RequiredOrbCount
        );

        inventory.MainInventoryAddItem((orbData.Item_Id + 1).ToString());

        // UI 갱신
        view.RefreshOrbPanel();
        view.ClearAllSlots();

        // 업그레이드된 오브 데이터 조회 및 알림 표시
        var upgradedData = DataTableManager.Instance
            .Get<ItemTable>(DataTableManager.item)
            .GetItemData((orbData.Item_Id + 1).ToString());

        model.CurrentOrbData = upgradedData;

        inventory.RefreshItemSlotUI();
        view.ShowNoticePanel(upgradedData);

        SoundManager.Instance.PlaySound2D("orb");
    }

    public void OnViewDisabled()
    {
        view.RefreshOrbPanel();
    }

    public void Dispose()
    {
        view.OnSlotClicked -= HandleSlotClicked;
        view.OnUpgradeButtonClicked -= HandleUpgrade;
    }
}
