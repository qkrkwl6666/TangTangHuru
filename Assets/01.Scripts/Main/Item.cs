using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Item
{
    public int ItemId { get; set; } // ������ Id
    public ItemTier Tier { get; set; } // ������ Ÿ��

    public void EquipItem();    // ��� ����
    public void UnEquipItem();  // ��� ����

    public void GetItemInfo(); // ������ ���� ����

}
