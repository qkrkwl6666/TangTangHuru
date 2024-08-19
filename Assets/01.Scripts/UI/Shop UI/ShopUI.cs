using Spine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public MainInventory inventory;
    public ShopEntry shopEntryPrefab;
    public GameObject shopContent;

    private List<ItemData> shopItems = new List<ItemData>();
    private List<ShopEntry> shopEntries = new List<ShopEntry>();
    private bool sorted = false;

    private void OnEnable()
    {
        if (sorted)
            return;

        //�ǸŸ��
        SetShopItems();

        foreach (ItemData item in shopItems)
        {
            SetEntry(item);
        }
        
        sorted = true;
    }

    private void SetShopItems()
    {
        shopItems.Add(DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData("720001"));
        shopItems.Add(DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData("720002"));
        shopItems.Add(DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData("720003"));
        shopItems.Add(DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData("720004"));
        shopItems.Add(DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData("600006"));
    }

    private void SetEntry(ItemData itemData)
    {
        var entry = Instantiate(shopEntryPrefab, shopContent.transform);
        entry.SetInfo(itemData.Texture_Id, itemData.Name_Id, itemData.Desc_Id, itemData.Price);
        entry.purchaseButton.onClick.AddListener(() => Purchase(itemData));
        shopEntries.Add(entry);
    }

    private void Purchase(ItemData itemData)
    {
        inventory.MainInventoryAddItem(itemData.Item_Id.ToString());
        inventory.Gold -= itemData.Price;
        inventory.RefreshGoldDiamondTextUI();
    }
}
