public interface Item
{
    public int ItemId { get; set; } // 아이템 Id
    public int InstanceId { get; set; }

    public ItemType ItemType { get; set; }
    public ItemTier ItemTier { get; set; } // 아이템 티어

    public float CurrentTierUp { get; set; }

    public ItemData itemData { get; set; }

    //public void EquipItem();    // 장비 장착
    //public void UnEquipItem();  // 장비 해제

    public void GetItemInfo(); // 아이템 정보 리턴

    public int CompareTo(Item other)
    {
        return this.InstanceId.CompareTo(other.InstanceId);
    }
}
