using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public Button appraisalButton;

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

        foreach(var gemStone in gemStoneSlotUI)
        {
            gemStone.Value.gameObject.SetActive(false);
        }

        foreach(var gemStone in allGemStone)
        {
            gemStoneSlotUI[gemStone.Key].SetActive(true);
            gemStoneSlotUI[gemStone.Key].GetComponent<M_GemStoneUISlot>()
                .RefreshItemCount(allGemStone[gemStone.Key].Count);
        }

        /*
            Normal,
            Rare,
            Epic,
            Unique,
            Legendary
         */
    }

    public void CreateGemstoneSlot()
    {
        // selectGemStone 초기 세팅
        for (int i = 0; i < (int)ItemTier.Count; i++)
        {
            selectGemStone.Add((ItemTier)i, 0);
        }

        for(int i = 0; i < (int)ItemTier.Count; i++)
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

        foreach(var apprasie in apprasieTable)
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
        if(allGemStone.ContainsKey(itemTier))
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

    public void RandomItemCreate(AppraiseData appraiseData)
    {
        List<(ItemType type, ItemTier tier, float prob)> list = new();

        foreach (var appraise in appraiseData.GetAppraiseData())
        {
            if (appraise.prob != -1)
            {
                list.Add(appraise);
            }
        }

        float totalProbability = 0f;
        float currentProbability = 0f;

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

    }

    public int SelectItem(ItemType itemType, ItemTier itemTier)
    {
        int maxCount = 0;

        switch (itemType)
        {
            case ItemType.Weapon:
                maxCount = (int)WeaponType.Count;

                int random = Random.Range(1, maxCount + 1);

                int itemId = (int)WeaponType.Axe + random;

                Debug.Log(itemId);
                return itemId;

            case ItemType.Helmet:
            case ItemType.Armor:
            case ItemType.Shose:

                return default;
        }

        return default;

    }

}
