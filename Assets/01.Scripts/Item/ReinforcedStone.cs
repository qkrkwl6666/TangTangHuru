using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 강화석
public class ReinforcedStone : IInGameItem
{
    public int ItemId { get; set; } = 600006;
    public string Name { get; set; } = DataTableManager.Instance.Get<StringTable>
        (DataTableManager.String).Get("Item_Name_Normal_Re_Stone").Text;
    public ItemType ItemType { get; set; } = ItemType.ReinforcedStone;

    public void GetItem()
    {
        
    }

    public void UseItem()
    {
        // Todo : 바로 메인 인벤토리 에 강화석이 들어가야함
        
    }
}
