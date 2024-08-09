using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class M_EquipmentGemstones : Item
{
    public int ItemId { get; set ; }
    public int InstanceId { get ; set ; }
    public ItemTier Tier { get ; set ; }

    public ItemData itemData { get; set ; }

    public void SetItemData(ItemData itemData, int instanceId)
    {
        ItemId = itemData.Item_Id;
        InstanceId = instanceId;
        Tier = (ItemTier)itemData.Item_Tier;

        this.itemData = itemData.DeepCopy();
    }

    public void EquipItem()
    {
        
    }

    public void GetItemInfo()
    {
        
    }

    public void UnEquipItem()
    {
        
    }
}
