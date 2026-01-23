using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// OrbCrafter의 View - UI만 담당
/// 모든 비즈니스 로직은 OrbCrafterPresenter에서 처리
/// </summary>
public class OrbCrafterView : MonoBehaviour, IOrbCrafterView
{
    [Header("Dependencies")]
    public OrbUpgraderView orbUpgraderView;
    public MainInventory inventory;

    [Header("UI Elements")]
    public TextMeshProUGUI stoneCountText;
    public TextMeshProUGUI stonePersent;
    public OrbNoticePanel popUp_Notice;
    public Button craftButton;
    public List<Image> gaige;

    [Header("Craft Settings")]
    [Tooltip("게이지 최대 충전 횟수")]
    [SerializeField] private int maxGaige = 4;

    [Tooltip("제작에 필요한 강화석 개수")]
    [SerializeField] private int stonesPerCraft = 3;

    [Tooltip("제작 성공 확률 (%)")]
    [SerializeField] [Range(0, 100)] private int createPercent = 56;

    [Tooltip("생성 가능한 오브 ID 목록")]
    [SerializeField] private List<int> orbIdList = new List<int> { 610001, 610101, 610201, 610301 };

    // Presenter
    private OrbCrafterPresenter presenter;

    // IOrbCrafterView 이벤트
    public event Action OnCraftButtonClicked;

    void Start()
    {
        // Model 생성 시 Inspector 설정값 주입
        var model = new OrbCrafterModel(maxGaige, stonesPerCraft, createPercent, orbIdList);

        // Presenter 생성
        var inventoryService = new MainInventoryAdapter(inventory);
        presenter = new OrbCrafterPresenter(this, model, inventoryService);

        // 버튼 이벤트 연결
        craftButton.onClick.AddListener(() => OnCraftButtonClicked?.Invoke());

        // 초기화
        presenter.Initialize();
    }

    private void OnEnable()
    {
        presenter?.OnViewEnabled();
    }

    private void OnDestroy()
    {
        presenter?.Dispose();
        craftButton.onClick.RemoveAllListeners();
    }

    #region IOrbCrafterView 구현

    public void SetStoneCount(int count)
    {
        stoneCountText.text = count.ToString();
    }

    public void SetSuccessPercent(int percent)
    {
        stonePersent.text = $"{percent}%";
    }

    public void UpdateGaige(int filledCount, int maxCount)
    {
        for (int i = 0; i < gaige.Count; i++)
        {
            gaige[i].color = i < filledCount ? Color.yellow : Color.clear;
        }
    }

    public void ResetGaige()
    {
        foreach (var img in gaige)
        {
            img.color = Color.clear;
        }
    }

    public void ShowNoticePanel(ItemData itemData)
    {
        popUp_Notice.SetInfo(itemData);
        popUp_Notice.gameObject.SetActive(true);
    }

    public void RefreshOrbPanel()
    {
        orbUpgraderView?.popUp_OrbPanel.ResetOn();
    }

    #endregion

    #region 외부 호출용 Public API

    /// <summary>
    /// 외부에서 성공 확률을 증가시킬 때 사용
    /// </summary>
    public void CreatePersentIncrease(int num)
    {
        presenter?.IncreasePercent(num);
    }

    #endregion
}
