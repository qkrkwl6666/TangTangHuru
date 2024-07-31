using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Item
{
    public int ItemId { get; set; } // 아이템 Id
    public ItemTier Tier { get; set; } // 아이템 타입

    public void EquipItem();    // 장비 장착
    public void UnEquipItem();  // 장비 해제

    public void GetItemInfo(); // 아이템 정보 리턴

}
