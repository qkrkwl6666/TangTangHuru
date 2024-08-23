using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class EquipmentAppraisal : MonoBehaviour
{
    public MainInventory mainInventory;

    private Dictionary<ItemTier, GameObject> gemStoneSlotUI = new();

    private Dictionary<ItemTier, int> selectGemStone = new();

    private Dictionary<ItemTier, List<Item>> allGemStone;

    private List<int> appraisalId = new();

    public Transform content;

    private int defaultGemStoneItemId = 600001;
    private int defaultAppraisalItemId = 290001;

    public AppraisalPopUp appraisalPopUp;

    public Button appraisalButton;
    public VFXRenderManager renderManager;
    public MainUI mainUI;

    private void Awake()
    {
        appraisalButton.onClick.AddListener(() => StartCoroutine(OnAppraisal()));
    }

    private void OnEnable()
    {
        foreach (var key in selectGemStone.Keys.ToList())
        {
            selectGemStone[key] = 0;
        }
    }

    private void Start()
    {
        // 원석 아이템 슬롯 생성

        if (!DataTableManager.Instance.isTableLoad)
            DataTableManager.Instance.OnAllTableLoaded += CreateGemstoneSlot;
        else
        {
            CreateGemstoneSlot();
        }
        //CreateGemstoneSlot();
    }

    private void OnDestroy()
    {

    }

    public void RefreshGemStoneSlotUI()
    {
        allGemStone = mainInventory.GetItemTypes(ItemType.EquipmentGem);

        if (allGemStone == null) return;

        foreach (var gemStone in gemStoneSlotUI)
        {
            gemStone.Value.gameObject.SetActive(false);
        }

        foreach (var gemStone in allGemStone)
        {
            if (gemStone.Value.Count == 0) continue;

            gemStoneSlotUI[gemStone.Key].SetActive(true);
            gemStoneSlotUI[gemStone.Key].GetComponent<M_GemStoneUISlot>()
                .RefreshItemCount(allGemStone[gemStone.Key].Count);

            gemStoneSlotUI[gemStone.Key].GetComponent<M_GemStoneUISlot>().SubButtonActive(false);
        }

    }

    public void CreateGemstoneSlot()
    {
        // selectGemStone 초기 세팅
        for (int i = 0; i < (int)ItemTier.Count; i++)
        {
            selectGemStone.Add((ItemTier)i, 0);
        }

        for (int i = 0; i < (int)ItemTier.Count; i++)
        {
            int tempItemId = defaultGemStoneItemId;
            ItemTier tempType = (ItemTier)i;

            Addressables.InstantiateAsync(Defines.gemStoneSlot, content).Completed
                += (x) =>
                {
                    var gemStone = x.Result;

                    var item = DataTableManager.Instance.Get<ItemTable>(DataTableManager.item)
                    .GetItemData(tempItemId.ToString());

                    gemStone.GetComponent<M_GemStoneUISlot>().SetItemData(item, this);
                    gemStoneSlotUI.Add(tempType, gemStone);

                    gemStone.SetActive(false);
                };

            defaultGemStoneItemId++;
        }

        var apprasieTable = DataTableManager.Instance.Get<AppraiseTable>(DataTableManager.appraise).appraiseTable;

        foreach (var apprasie in apprasieTable)
        {
            appraisalId.Add(apprasie.Value.Id);
        }

        gameObject.SetActive(false);
    }

    public int GetSelectGemStoneCount(ItemTier itemTier)
    {
        return selectGemStone[itemTier];
    }

    public int GetMaxGemStoneCount(ItemTier itemTier)
    {
        if (allGemStone.ContainsKey(itemTier))
        {
            return allGemStone[itemTier].Count;
        }

        return default;
    }

    public bool AddGemStone(ItemTier itemTier)
    {
        if (selectGemStone[itemTier] >= allGemStone[itemTier].Count)
            return false;

        selectGemStone[itemTier]++;

        return true;
    }

    public bool SubGemStone(ItemTier itemTier)
    {
        if (selectGemStone[itemTier] <= 0)
            return false;

        selectGemStone[itemTier]--;

        return true;
    }

    public Item RandomItemCreate(AppraiseData appraiseData)
    {
        List<(ItemType type, ItemTier tier, float prob)> list = new();

        float totalProbability = 0f;
        float currentProbability = 0f;

        foreach (var appraise in appraiseData.GetAppraiseData())
        {
            if (appraise.prob != -1)
            {
                list.Add(appraise);
                totalProbability += appraise.prob;
            }
        }

        float random = Random.Range(0, totalProbability);

        int itemId = 0;

        foreach (var appraise in list)
        {
            currentProbability += appraise.prob;

            if (random <= currentProbability)
            {
                itemId = SelectItem(appraise.type, appraise.tier);
                break;
            }
        }

        // 아이템 생성후 정보 띄우기

        if (itemId == -1) return null;

        var item = mainInventory.MainInventoryAddItem(itemId.ToString());

        return item;

    }

    //
    // WeaponType 쪽 데이터 테이블 매니저 에서 무기 타입 매개변수로 전달하면 
    // 자동으로 무기 타입 중에서 랜덤으로 뽑는 메서드 만들기

    public int SelectItem(ItemType itemType, ItemTier itemTier)
    {

        switch (itemType)
        {
            case ItemType.Weapon:

                ItemType weaponRandomType = (ItemType)Random.Range(1, Defines.MaxWeaponCount + 1);

                var item = DataTableManager.Instance.Get<ItemTable>(DataTableManager.item)
                    .GetItemData(weaponRandomType, itemTier);
                return item.Item_Id;

            case ItemType.Helmet:
                {
                    ArmorType armorType = Defines.GetRandomArmorType();
                    int itemId = Defines.GetArmorId(armorType, ItemType.Helmet, itemTier);
                    return itemId;
                }

            case ItemType.Armor:
                {
                    ArmorType armorType = Defines.GetRandomArmorType();
                    int itemId = Defines.GetArmorId(armorType, ItemType.Armor, itemTier);
                    return itemId;
                }

            case ItemType.Shose:
                {
                    ArmorType armorType = Defines.GetRandomArmorType();
                    int itemId = Defines.GetArmorId(armorType, ItemType.Shose, itemTier);
                    return itemId;
                }
        }

        return default;

    }

    // 감정 버튼 누르면 호출
    public System.Collections.IEnumerator OnAppraisal()
    {
        // 내가 현재 선택한 원석들 가져와서 한번에 다 뽑고 n번 팝업창 띄우기

        List<Item> itemList = new();

        foreach (var appraisal in selectGemStone)
        {
            for (int i = 0; i < appraisal.Value; i++)
            {
                int appraiseId = defaultAppraisalItemId + (int)appraisal.Key;

                AppraiseData appraiseData = DataTableManager.Instance
                    .Get<AppraiseTable>(DataTableManager.appraise)
                    .GetData(appraiseId.ToString());

                Item item = RandomItemCreate(appraiseData);

                itemList.Add(item);

            }
        }

        if (itemList.Count <= 0) yield break;

        // 선택한 재화 소모하기
        for (int i = 0; i < (int)ItemTier.Count; i++)
        {
            int removeCount = selectGemStone[(ItemTier)i];

            if (removeCount == 0) continue;

            mainInventory.RemoveItem(ItemType.EquipmentGem, (ItemTier)i, removeCount);

            RefreshGemStoneSlotUI();
        }

        foreach (var key in selectGemStone.Keys.ToList())
        {
            selectGemStone[key] = 0;
        }

        mainInventory.SaveInventory();

        renderManager.PlayAppraisalParticle(3f);
        mainUI.allCloseUI.SetActive(true);

        yield return new WaitForSeconds(3f);

        mainUI.allCloseUI.SetActive(false);
        appraisalPopUp.gameObject.SetActive(true);
        appraisalPopUp.SetPopUp(itemList);

    }

}
