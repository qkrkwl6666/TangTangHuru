using System.Collections.Generic;

public class M_Weapon : Item
{
    public int ItemId { get ; set ; }
    public int InstanceId { get ; set ; }
    public ItemType ItemType { get; set; }
    public ItemTier ItemTier { get ; set ; }
    public ItemData itemData { get ; set ; }

    public List<Item> subWeapons { get ; set ; } // 가지고 있는 무기
    public List<Item> orbs { get ; set ; } // 보유하고 있는 오브

    public void GetItemInfo()
    {
        
    }

    public void SetItemData(ItemData itemData, int instanceId)
    {
        ItemId = itemData.Item_Id;
        InstanceId = instanceId;

        ItemType = (ItemType)itemData.Item_Type;
        ItemTier = (ItemTier)itemData.Item_Tier;

        this.itemData = itemData;
    }

    public void UpgradeWeapon(int UpgradeCount)
    {
        if(UpgradeCount == 0 && itemData.CurrentUpgrade >= 10) return;

        var initItemData = DataTableManager.Instance.Get<ItemTable>
            (DataTableManager.item).GetItemData(itemData.Item_Id.ToString());

        float initDamage = initItemData.Damage;
        float initCooldown = initItemData.CoolDown;
        float initCriticalChance = initItemData.CriticalChance;
        float initCriticalDamage = initItemData.Criticaldam;

        for (int i = 0; i < UpgradeCount; i++)
        {
            initDamage += initItemData.Damage * initItemData.UpDamage;
            initCooldown -= initItemData.CoolDown * initItemData.UpCoolDown;
            initCriticalChance += initItemData.CriticalChance * initItemData.UpCriticalChance;
            initCriticalDamage += initItemData.Criticaldam * initItemData.UpCriticalDam;
        }

        if (initCooldown <= 0) initCooldown = 0.1f;

        itemData.CurrentUpgrade = UpgradeCount;

        itemData.Damage = initDamage;
        itemData.CoolDown = initCooldown;
        itemData.CriticalChance = initCriticalChance;
        itemData.Criticaldam = initCriticalDamage;

        // UI 적용
    }

    public List<Item> GetSubWeapon()
    {
        return subWeapons;
    }

}
