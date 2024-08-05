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
        // ������ Ÿ���� ���ٸ�

        if (!allItem.ContainsKey(itemType)) 
        {
            allItem.Add(itemType, new SortedDictionary<ItemTier, List<Item>>());
            allItem[itemType].Add(itemTier, new List<Item>());

            allItem[itemType][itemTier].Add(mainItem);
            return;
        }

        // ������ Ÿ���� �ְ� ������ Ƽ� ���ٸ� 
        if (!allItem[itemType].ContainsKey(itemTier))
        {
            allItem[itemType].Add(itemTier, new List<Item>());

            allItem[itemType][itemTier].Add(mainItem);
            return;
        }

        // ������ Ÿ�԰� ������ Ƽ� �ִٸ�
        {
            allItem[itemType][itemTier].Add(mainItem);
            return;
        }

    }

    public Item MakeItem(ItemData itemData)
    {
        switch(itemData.Item_Type)
        {
            case 1: // ����

                break;
            case 2: // ����

                break;
            case 3: // ����

                break;
            case 4: // �Ź�

                break;
            case 5: // ��� ����
                M_EquipmentGemstones m_EquipmentGemstones = new M_EquipmentGemstones();
                int instanceId = m_EquipmentGemstones.GetHashCode() + Random.Range(1, 100000);

                m_EquipmentGemstones.SetItemData(itemData, instanceId);  
                
                return m_EquipmentGemstones;
            case 6: // ��ȭ��

                break;
        }

        return null;
    }


}

public enum ItemType
{
    Weapon = 1, // ����
    Helmet = 2, // ����
    Armor = 3, // ����
    Shose = 4, // �Ź�
    EquipmentGem = 5, // ��� ����
    ReinforcedStone = 6, // ��ȭ��
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
