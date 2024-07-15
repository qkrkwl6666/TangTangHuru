using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class EquipmentGemstone : IInGameItem
{
    public int ItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ItemType ItemType { get; set; } = ItemType.EquipmentGemstone;

    public EquipmentGemstone()
    {

    }

    public EquipmentGemstone(ItemData itemdata)
    {
        ItemId = itemdata.Item_Id;
        Name = DataTableManager.Instance.Get<StringTable>(DataTableManager.String)
            .Get(itemdata.Name_Id).Text;
    }

    public void GetItem()
    {

    }

    public void UseItem()
    {
        // 인게임 인벤토리에 들어가야 함
    }
}
