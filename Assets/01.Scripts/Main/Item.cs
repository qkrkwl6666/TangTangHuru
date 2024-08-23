public interface Item
{
    public int ItemId { get; set; } // ������ Id
    public int InstanceId { get; set; }

    public ItemType ItemType { get; set; }
    public ItemTier ItemTier { get; set; } // ������ Ƽ��

    public float CurrentTierUp { get; set; }

    public ItemData itemData { get; set; }

    //public void EquipItem();    // ��� ����
    //public void UnEquipItem();  // ��� ����

    public void GetItemInfo(); // ������ ���� ����

    public int CompareTo(Item other)
    {
        return this.InstanceId.CompareTo(other.InstanceId);
    }
}
