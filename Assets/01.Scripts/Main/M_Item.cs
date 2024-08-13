public class M_Item : Item
{
    public int ItemId { get; set ; }
    public int InstanceId { get ; set ; }
    public ItemType ItemType { get; set; }
    public ItemTier ItemTier { get ; set ; }
    public ItemData itemData { get ; set ; }
    public float CurrentTierUp { get ; set ; }

    public void SetItemData(ItemData itemData, int instanceId)
    {
        ItemId = itemData.Item_Id;
        InstanceId = instanceId;

        this.ItemType = (ItemType)itemData.Item_Type;
        this.ItemTier = (ItemTier)itemData.Item_Tier;

        this.itemData = itemData;
    }

    public void GetItemInfo()
    {

    }
}
