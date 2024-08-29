using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public MainInventory inventory;
    public ShopEntry shopEntryPrefab;
    public GameObject shopContent;

    private List<ItemData> shopItems = new List<ItemData>();
    private List<ShopEntry> shopEntries = new List<ShopEntry>();
    private bool sorted = false;

    private bool isCheatOn = false;
    private void OnEnable()
    {
        if (sorted)
            return;

        //판매목록
        SetShopItems();

        foreach (ItemData item in shopItems)
        {
            SetEntry(item);
        }

        sorted = true;
    }

    private void OnDisable()
    {
        //inventory.RefreshItemSlotUI();
    }

    private void SetShopItems()
    {
        shopItems.Add(DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData("720001"));
        shopItems.Add(DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData("720002"));
        shopItems.Add(DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData("720003"));
        shopItems.Add(DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData("720004"));
        shopItems.Add(DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData("600006"));
        shopItems.Add(DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData("220101"));
        shopItems.Add(DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData("210004"));

        shopItems.Add(DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData("610001"));
        shopItems.Add(DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData("610102"));
        shopItems.Add(DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData("610202"));
        shopItems.Add(DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData("610301"));

        shopItems.Add(DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData("400004"));
        shopItems.Add(DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData("401003"));
        shopItems.Add(DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData("402005"));



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
        if (inventory.Gold < itemData.Price && !isCheatOn)
        {
            SoundManager.Instance.PlaySound2D("failed");
        }
        else
        {
            inventory.MainInventoryAddItem(itemData.Item_Id.ToString());
            inventory.Gold -= itemData.Price;
            inventory.RefreshGoldDiamondTextUI();

            inventory.RefreshItemSlotUI();

            SoundManager.Instance.PlaySound2D("success");

            AchievementManager.Instance.myTasks.AddProgress("TotalPurchase");
            if (AchievementManager.Instance.Check("TotalPurchase"))
            {
                AchievementManager.Instance.UnlockAchievement("TotalPurchase");
            }

            AchievementManager.Instance.myTasks.AddProgress("UsedGold", itemData.Price);
            if (AchievementManager.Instance.Check("UsedGold"))
            {
                AchievementManager.Instance.UnlockAchievement("UsedGold");
            }

        }

    }

    public void CheatOn()
    {
        isCheatOn = !isCheatOn;
    }
}
