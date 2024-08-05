using System.Collections.Generic;
using UnityEngine;

public class MainInventory : MonoBehaviour
{
    // 전체 아이템 컨테이너
    private SortedDictionary<ItemType, SortedDictionary<ItemTier, List<Item>>> allItem = new ();

    // 플레이어가 가지고있는 아이템 컨테이너 장비 
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
    OneHandedSword, // 한손검
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
    Helmet, // 투구
    Armor, // 갑옷
    Shoes, // 신발
}
