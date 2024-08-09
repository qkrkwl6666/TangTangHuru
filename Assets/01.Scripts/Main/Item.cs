public interface Item
{
    public int ItemId { get; set; } // 아이템 Id
    public int InstanceId { get; set; }
    public ItemTier Tier { get; set; } // 아이템 티어

    public void EquipItem();    // 장비 장착
    public void UnEquipItem();  // 장비 해제

    public void GetItemInfo(); // 아이템 정보 리턴
}
