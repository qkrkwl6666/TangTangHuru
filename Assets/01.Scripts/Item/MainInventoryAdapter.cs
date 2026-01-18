using System.Collections.Generic;

/// <summary>
/// MainInventory를 IInventoryService로 변환하는 Adapter 패턴 구현
/// Presenter가 MainInventory 클래스 의존성 감소
/// </summary>
public class MainInventoryAdapter : IInventoryService
{
    private readonly MainInventory inventory;

    public MainInventoryAdapter(MainInventory inventory)
    {
        this.inventory = inventory;
    }

    public int GetItemCount(ItemType type, ItemTier tier)
    {
        return inventory.GetItemCount(type, tier);
    }

    public void RemoveItem(ItemType type, ItemTier tier, int count)
    {
        inventory.RemoveItem(type, tier, count);
    }

    public bool RemoveOrbItem(ItemType type, ItemTier tier, int count)
    {
        return inventory.RemoveOrbItem(type, tier, count);
    }

    public Item MainInventoryAddItem(string itemId)
    {
        return inventory.MainInventoryAddItem(itemId);
    }

    public void RefreshItemSlotUI()
    {
        inventory.RefreshItemSlotUI();
    }

    public List<Item> GetItemTypesTier(ItemType type, ItemTier tier)
    {
        return inventory.GetItemTypesTier(type, tier);
    }
}
