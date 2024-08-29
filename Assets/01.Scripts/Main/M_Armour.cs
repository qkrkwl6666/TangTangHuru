using System.Collections.Generic;

public class M_Armour : Item
{
    public int ItemId { get; set; }
    public int InstanceId { get; set; }
    public ItemType ItemType { get; set; }
    public ItemTier ItemTier { get; set; }
    public ItemData itemData { get; set; }
    public List<Item> orbs { get; set; } // 보유하고 있는 오브
    public float CurrentTierUp { get; set; }

    public void GetItemInfo()
    {

    }

    public void UpgradeArmor(int UpgradeCount)
    {
        var initItemData = DataTableManager.Instance.Get<ItemTable>
                (DataTableManager.item).GetItemData(itemData.Item_Id.ToString());

        float value = 0f;
        float upgradeValue = 0f;

        switch (itemData.Item_Type)
        {
           case (int)ItemType.Helmet:
                value = initItemData.Defense;
                upgradeValue = initItemData.Defense;
                break;

            case (int)ItemType.Armor:
                value = initItemData.Hp;
                upgradeValue = initItemData.Hp;
                break;

            case (int)ItemType.Shose:
                value = initItemData.Dodge;
                upgradeValue = initItemData.Dodge;
                break;
        }

        for (int i = 0; i < UpgradeCount; i++)
        {
            value += upgradeValue * initItemData.Damagecal;
        }

        switch (itemData.Item_Type)
        {
            case (int)ItemType.Helmet:
;               itemData.Defense = value;
                break;
            case (int)ItemType.Armor:
                itemData.Hp = value;
                break;
            case (int)ItemType.Shose:
                itemData.Dodge = value;
                break;
        }

        itemData.CurrentUpgrade = UpgradeCount;


    }

    public void SetItemData(ItemData itemData, int instanceId)
    {
        ItemId = itemData.Item_Id;
        InstanceId = instanceId;

        ItemType = (ItemType)itemData.Item_Type;
        ItemTier = (ItemTier)itemData.Item_Tier;

        this.itemData = itemData;
    }
}
