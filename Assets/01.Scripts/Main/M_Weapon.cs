using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Weapon : Item
{
    public int ItemId { get ; set ; }
    public int InstanceId { get ; set ; }
    public ItemType ItemType { get; set; }
    public ItemTier ItemTier { get ; set ; }
    public ItemData itemData { get ; set ; }

    public List<Item> orbs { get ; set ; } // 보유하고 있는 오브

    public void GetItemInfo()
    {
        
    }

    public void SetItemData(ItemData itemData, int instanceId)
    {
        ItemId = itemData.Item_Id;
        InstanceId = instanceId;
        ItemTier = (ItemTier)itemData.Item_Tier;

        this.itemData = itemData.DeepCopy();
    }

    public void UpgradeWeapon()
    {

    }

}
