using System.Collections.Generic;
using UnityEngine;

public class MainInventory : MonoBehaviour
{
    // ��ü ������ �����̳�
    private SortedDictionary<ItemType, SortedDictionary<ItemTier, List<Item>>> allItem = new ();

    // �÷��̾ �������ִ� ������ �����̳� ��� 
    private Dictionary<PlayerEquipment, Item> playerEquipment = new ();

    private void Awake()
    {
        //var ab = allItem[ItemType.Bow][TierType.Rare][0];
    }


    public void MainInventoryAddItem(string itemId)
    {

    }


}

public enum ItemType
{
    None,
    OneHandedSword, // �Ѽհ�
    Axe,
    Bow,
    Crossbow,
    Wand,
    Staff,
}

public enum ItemTier
{
    Normal,
    Rare,
    Epic,
    Unique,
    Legendary
}

public enum PlayerEquipment
{
    Weapon = 1,
    Helmet, // ����
    Armor, // ����
    Shoes, // �Ź�
}
