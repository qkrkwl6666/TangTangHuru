using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class EquipmentAppraisal : MonoBehaviour
{
    public MainInventory mainInventory;

    private Dictionary<ItemTier, GameObject> gemStoneSlotUI = new();

    private Dictionary<ItemTier, int> selectGemStone = new();

    private Dictionary<ItemTier, List<Item>> allGemStone;

    public Transform content;

    private int defaultGemStoneItemId = 600001;

    private void Start()
    {
        // 원석 아이템 슬롯 생성

        DataTableManager.Instance.OnAllTableLoaded += CreateGemstoneSlot;

        //CreateGemstoneSlot();
    }

    private void OnDestroy()
    {
        DataTableManager.Instance.OnAllTableLoaded -= CreateGemstoneSlot;
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

        gameObject.SetActive(false);
    }

    public void SortGemStoneSlotUI()
    {

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
}
