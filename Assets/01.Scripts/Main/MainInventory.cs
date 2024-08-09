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

    public void CreateEquipmentGem()
    {
        MainInventoryAddItem(600001.ToString());
    }

    public void MainInventoryAddItem(string itemId)
    {
        var item = DataTableManager.Instance.Get<ItemTable>
            (DataTableManager.item).GetItemData(itemId);

        if (item == null) return;

        ItemType itemType = (ItemType)item.Item_Type;
        ItemTier itemTier = (ItemTier)item.Item_Tier;

        var mainItem = MakeItem(item);
        // 아이템 타입이 없다면

        if (!allItem.ContainsKey(itemType)) 
        {
            allItem.Add(itemType, new SortedDictionary<ItemTier, List<Item>>());
            allItem[itemType].Add(itemTier, new List<Item>());

            allItem[itemType][itemTier].Add(mainItem);
            return;
        }

        // 아이템 타입이 있고 아이템 티어가 없다면 
        if (!allItem[itemType].ContainsKey(itemTier))
        {
            allItem[itemType].Add(itemTier, new List<Item>());

            allItem[itemType][itemTier].Add(mainItem);
            return;
        }

        // 아이템 타입과 아이템 티어가 있다면
        {
            allItem[itemType][itemTier].Add(mainItem);
            return;
        }

    }

    public Item MakeItem(ItemData itemData)
    {
        switch(itemData.Item_Type)
        {
            case 1: // 무기

                break;
            case 2: // 투구

                break;
            case 3: // 갑옷

                break;
            case 4: // 신발

                break;
            case 5: // 장비 원석
                M_EquipmentGemstones m_EquipmentGemstones = new M_EquipmentGemstones();
                int instanceId = m_EquipmentGemstones.GetHashCode() + Random.Range(1, 100000);

                m_EquipmentGemstones.SetItemData(itemData, instanceId);  
                
                return m_EquipmentGemstones;
            case 6: // 강화석

                break;
        }

        return null;
    }


}

public enum ItemType
{
    Weapon = 1, // 무기
    Helmet = 2, // 투구
    Armor = 3, // 갑옷
    Shose = 4, // 신발
    EquipmentGem = 5, // 장비 원석
    ReinforcedStone = 6, // 강화석
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
