using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static UnityEditor.Progress;

public class MainInventory : MonoBehaviour
{
    // ��ü ������ �����̳�
    private Dictionary<ItemType, Dictionary<ItemTier, List<Item>>> allItem = new ();

    // �÷��̾ �������ִ� ������ �����̳� ��� 
    private Dictionary<PlayerEquipment, Item> playerEquipment = new ();

    // ���� ������ UI ���� ������
    private SortedList<int, (Item, GameObject ItemSlot)> itemSlotUI = new ();

    // �Ҹ�ǰ ������ �����̳�
    private Dictionary<string, (Item item, int count)> consumableItems = new();

    public Transform content;

    public List<int> items = new ();

    private MainUI mainUI;

    private void Awake()
    {
        //var ab = allItem[ItemType.Bow][TierType.Rare][0];

        mainUI = GameObject.FindWithTag("MainUI").GetComponent<MainUI>();

        items.Add(200001);
        items.Add(200002);
        items.Add(200003);
        items.Add(200004);
        items.Add(200005);
        items.Add(200101);
        items.Add(200102);
        items.Add(200103);
        items.Add(200104);
        items.Add(200105);
        items.Add(210001);
        items.Add(210002);
        items.Add(210003);
        items.Add(210004);
        items.Add(600001);
        items.Add(600002);
        items.Add(600003);
        items.Add(600004);
        items.Add(600005);
        items.Add(600006);
    }

    public void CreateEquipmentGem()
    {
        int random = Random.Range(0, items.Count);

        MainInventoryAddItem(items[random].ToString());

        RefreshItemSlotUI();
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
            allItem.Add(itemType, new Dictionary<ItemTier, List<Item>>());
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
        M_Item m_item = new M_Item();
        int instanceId = m_item.GetHashCode() + Random.Range(1, 100000);

        m_item.SetItemData(itemData, instanceId);

        return m_item;

        //switch (itemData.Item_Type)
        //{
        //    case 1: // ����

        //        break;
        //    case 2: // ����

        //        break;
        //    case 3: // ����

        //        break;
        //    case 4: // �Ź�

        //        break;
        //    case 5: // ��� ����
        //        //M_EquipmentGemstones m_EquipmentGemstones = new M_EquipmentGemstones();
        //        //int instanceId = m_EquipmentGemstones.GetHashCode() + Random.Range(1, 100000);

        //        //m_EquipmentGemstones.SetItemData(itemData, instanceId);  
                
        //        //return m_EquipmentGemstones;
        //    case 6: // ��ȭ��

        //        break;
        //}

        //return null;
    }

    // ���� ������ �ִ� ������ ui �� ���� �̹� ������ ������ ui��� �ѱ��
    // ���ΰ�ħ Ÿ�̹��� ���ӿ� ó�� ���ӽ� �� ���� ���� �� ���ξ� �̵� ��
    // �������� ȹ���
    public void RefreshItemSlotUI()
    {
        float firstTime = Time.time;

        // �Ҹ�ǰ ���� �ʱ�ȭ
        var keys = new List<string>(consumableItems.Keys);
        foreach (var key in keys)
        {
            consumableItems[key] = (consumableItems[key].item, 0);
        }

        foreach(var ItemTypeDictionary in allItem)
        {
            if (ItemTypeDictionary.Key == ItemType.Orb) continue;

            foreach (var ItemTierDictionary in ItemTypeDictionary.Value)
            {
                var itemList = ItemTierDictionary.Value;

                for(int i = 0; i < itemList.Count; i++)
                {
                    switch (ItemTypeDictionary.Key)
                    {
                        case ItemType.Weapon:
                        case ItemType.Helmet:
                        case ItemType.Armor:
                        case ItemType.Shose:
                            CreateOrUpdateItemSlot(itemList[i]);
                            break;

                        case ItemType.EquipmentGem:
                        case ItemType.ReinforcedStone:
                            UpdateConsumableItemCount(itemList[i]);
                            break;

                        case ItemType.Orb:
                            break;
                    }
                }
            }
        }

        // �Ҹ�ǰ UI ������Ʈ

        foreach(var kvp in consumableItems)
        {
            if(kvp.Value.count > 0)
            {
                CreateOrUpdateItemSlot(kvp.Value.item, true, kvp.Value.count);
            }
            else // �Ҹ�ǰ�� 0�� ��� ������ ����
            {
                RemoveItem(kvp.Value.item.ItemId);
            }
        }

        float time = Time.time - firstTime;

        Debug.Log($"�ɸ� �ð� : {time}");
    }

    public void CreateOrUpdateItemSlot(Item item, bool isConsumable = false, int itemCount = 0)
    {
        if(isConsumable)
        {
            if(!itemSlotUI.ContainsKey(item.ItemId))
            {
                Addressables.InstantiateAsync(Defines.itemSlot, content).Completed += (itemGo) =>
                {
                    var go = itemGo.Result;

                    go.GetComponent<M_UISlot>().SetItemData(item.itemData, mainUI);
                    go.GetComponent<M_UISlot>().SetItemDataConsumable(item.itemData, itemCount);

                    itemSlotUI.Add(item.ItemId, (item, go));
                };
            }
            else
            {
                itemSlotUI[item.ItemId].ItemSlot.GetComponent<M_UISlot>().SetItemDataConsumable(item.itemData, itemCount);
            }


            return;
        }

        if(!itemSlotUI.ContainsKey(item.InstanceId))
        {
            Addressables.InstantiateAsync(Defines.itemSlot, content).Completed += (itemGo) => 
            {
                var go = itemGo.Result;

                go.GetComponent<M_UISlot>().SetItemData(item.itemData, mainUI);

                itemSlotUI.Add(item.InstanceId, (item, go));
            };
        }
    }

    private void UpdateConsumableItemCount(Item item)
    {
        string key = item.itemData.Item_Id.ToString();
        if (consumableItems.ContainsKey(key))
        {
            var (existingItem, count) = consumableItems[key];
            consumableItems[key] = (existingItem, count + 1);
        }
        else
        {
            consumableItems[key] = (item, 1);
        }
    }

    // ��� ���� ������ ����
    public void RemoveItem(int instanceId)
    {
        if (!itemSlotUI.ContainsKey(instanceId)) return;

        var item = itemSlotUI[instanceId];

        // item UI ���� ����
        Destroy(item.ItemSlot);

        var itemData = item.Item1.itemData;

        var itemList = allItem[(ItemType)itemData.Item_Type][(ItemTier)itemData.Item_Tier];

        itemList.Remove(item.Item1);

        // �Ҹ�ǰ �̸� �Ҹ�ǰ �����̳ʵ� ������ ������ �����
    }

    // �Ҹ�ǰ ������ ����

}

public enum ItemType
{
    Weapon = 1, // ����
    Helmet = 2, // ����
    Armor = 3, // ����
    Shose = 4, // �Ź�
    EquipmentGem = 5, // ��� ����
    ReinforcedStone = 6, // ��ȭ��
    Orb = 7, // ����
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
