using System.Collections.Generic;

/// <summary>
/// MainInventory에 대한 추상화 인터페이스
/// Presenter가 MainInventory 구현에 의존하지 않도록 함
/// </summary>
public interface IInventoryService
{
    /// 특정 타입과 티어의 아이템 개수 조회
    int GetItemCount(ItemType type, ItemTier tier);

    /// 소모품(강화석 등) 제거
    void RemoveItem(ItemType type, ItemTier tier, int count);

    /// 오브 아이템 제거 (개별 인스턴스)
    bool RemoveOrbItem(ItemType type, ItemTier tier, int count);

    /// 아이템 추가
    Item MainInventoryAddItem(string itemId);

    /// 인벤토리 UI 갱신
    void RefreshItemSlotUI();

    /// 특정 타입과 티어의 아이템 리스트 조회
    List<Item> GetItemTypesTier(ItemType type, ItemTier tier);
}
