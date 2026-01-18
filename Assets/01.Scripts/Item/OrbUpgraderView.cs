using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// OrbUpgrader의 View - UI만 담당
/// 로직은 OrbUpgraderPresenter에서 처리
/// </summary>
public class OrbUpgraderView : MonoBehaviour, IOrbUpgraderView
{
    [Header("Dependencies")]
    public MainInventory inventory;

    [Header("UI Elements")]
    public ItemSlotUI[] upgradeSlots;
    public Button upgradeButton;
    public OrbPanel popUp_OrbPanel;
    public OrbNoticePanel popUp_Notice;

    [Header("Upgrade Settings")]
    [Tooltip("업그레이드에 필요한 동일 오브 개수")]
    [SerializeField] private int requiredOrbCount = 3;

    [Tooltip("업그레이드 가능한 최대 티어 (이 티어까지만 업그레이드 가능)")]
    [SerializeField] private int maxUpgradeTier = 3;

    private OrbUpgraderPresenter presenter;

    public event Action<ItemSlotUI> OnSlotClicked;
    public event Action OnUpgradeButtonClicked;

    private void OnEnable()
    {
        if (inventory == null)
        {
            var inventoryObj = GameObject.FindGameObjectWithTag("MainInventory");
            inventory = inventoryObj.GetComponent<MainInventory>();
        }

        upgradeSlots = GetComponentsInChildren<ItemSlotUI>();

        // Model 생성 시 Inspector 설정값 주입
        var model = new OrbUpgraderModel(requiredOrbCount, maxUpgradeTier);

        var inventoryService = new MainInventoryAdapter(inventory);
        presenter = new OrbUpgraderPresenter(this, model, inventoryService);

        foreach (var slot in upgradeSlots)
        {
            var capturedSlot = slot;
            slot.GetComponent<Button>().onClick.AddListener(() => OnSlotClicked?.Invoke(capturedSlot));
        }

        upgradeButton.onClick.AddListener(() => OnUpgradeButtonClicked?.Invoke());

        presenter.Initialize();
    }

    private void OnDisable()
    {
        // 슬롯 이벤트 정리
        foreach (var slot in upgradeSlots)
        {
            slot.GetComponent<Button>().onClick.RemoveAllListeners();
        }

        presenter?.OnViewDisabled();
        presenter?.Dispose();
    }

    #region IOrbUpgraderView 구현

    public void ClearAllSlots()
    {
        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            upgradeSlots[i].ClearInfo();
        }
    }

    public void ShowOrbPanel(ItemSlotUI targetSlot)
    {
        popUp_OrbPanel.currSlot = targetSlot;
        popUp_OrbPanel.gameObject.SetActive(true);
    }

    public void HideOrbPanel()
    {
        popUp_OrbPanel.gameObject.SetActive(false);
    }

    public void RefreshOrbPanel()
    {
        popUp_OrbPanel.ResetOn();
    }

    public void ShowNoticePanel(ItemData itemData)
    {
        popUp_Notice.SetInfo(itemData);
        popUp_Notice.gameObject.SetActive(true);
    }

    public ItemSlotUI[] GetUpgradeSlots()
    {
        return upgradeSlots;
    }

    #endregion

    #region 외부 호출용 Public API

    /// OrbPanel에서 오브 선택 시 호출
    public void SelectOrbInPanel(int index)
    {
        presenter?.SelectOrbInPanel(index, popUp_OrbPanel.orbList, popUp_OrbPanel.currSlot);
    }

    /// WeaponOrbSlot 등에서 슬롯 선택 시 호출
    public void SlotSelected(ItemSlotUI slot)
    {
        OnSlotClicked?.Invoke(slot);
    }

    #endregion
}
