public interface Item
{
    public int ItemId { get; set; } // ������ Id
    public int InstanceId { get; set; }
    public ItemTier Tier { get; set; } // ������ Ƽ��

    public void EquipItem();    // ��� ����
    public void UnEquipItem();  // ��� ����

    public void GetItemInfo(); // ������ ���� ����
}
