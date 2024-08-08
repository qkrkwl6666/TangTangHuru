using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class EquipmentAppraisal : MonoBehaviour
{
    public MainInventory mainInventory;

    private Dictionary<ItemTier, GameObject> gemStoneSlotUI = new();

    public Transform content;

    private int defaultGemStoneItemId = 600001;

    private void Start()
    {
        // 원석 아이템 슬롯 생성

        DataTableManager.Instance.OnAllTableLoaded += CreateGemstoneSlot;

        //CreateGemstoneSlot();
    }

    public void RefreshGemStoneSlotUI()
    {
        var gemStoneDic = mainInventory.GetItemTypes(ItemType.EquipmentGem);

        for(int i = 0; i < gemStoneDic.Count; i++)
        {
            var items = gemStoneDic[(ItemTier)i];
        }

        /*
            Normal,
            Rare,
            Epic,
            Unique,
            Legendary
         */

        foreach (var itemTier in gemStoneDic)
        {

        }
    }

    public void CreateGemstoneSlot()
    {
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

                    gemStone.GetComponent<M_GemStoneUISlot>().SetItemData(item);
                    gemStoneSlotUI.Add(tempType, gemStone);
                };

            defaultGemStoneItemId++;
        }

        gameObject.SetActive(false);
    }

    public void SortGemStoneSlotUI()
    {

    }
}
