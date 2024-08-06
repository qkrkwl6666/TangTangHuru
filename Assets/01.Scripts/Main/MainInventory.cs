using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static UnityEditor.Progress;

public class MainInventory : MonoBehaviour
{
    // 전체 아이템 컨테이너
    private Dictionary<ItemType, Dictionary<ItemTier, List<Item>>> allItem = new ();

    // 플레이어가 가지고있는 아이템 컨테이너 장비 
    private Dictionary<PlayerEquipment, Item> playerEquipment = new ();

    // 현재 생성된 UI 슬롯 아이템
    private SortedList<int, (Item, GameObject ItemSlot)> itemSlotUI = new ();

    // 소모품 아이템 컨테이너
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
        // 아이템 타입이 없다면

        if (!allItem.ContainsKey(itemType)) 
        {
            allItem.Add(itemType, new Dictionary<ItemTier, List<Item>>());
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
        M_Item m_item = new M_Item();
        int instanceId = m_item.GetHashCode() + Random.Range(1, 100000);

        m_item.SetItemData(itemData, instanceId);

        return m_item;

        //switch (itemData.Item_Type)
        //{
        //    case 1: // 무기

        //        break;
        //    case 2: // 투구

        //        break;
        //    case 3: // 갑옷

        //        break;
        //    case 4: // 신발

        //        break;
        //    case 5: // 장비 원석
        //        //M_EquipmentGemstones m_EquipmentGemstones = new M_EquipmentGemstones();
        //        //int instanceId = m_EquipmentGemstones.GetHashCode() + Random.Range(1, 100000);

        //        //m_EquipmentGemstones.SetItemData(itemData, instanceId);  
                
        //        //return m_EquipmentGemstones;
        //    case 6: // 강화석

        //        break;
        //}

        //return null;
    }

    // 현재 가지고 있는 아이템 ui 에 생성 이미 생성된 아이템 ui라면 넘기기
    // 새로고침 타이밍은 게임에 처음 접속시 인 게임 종료 후 메인씬 이동 시
    // 아이템을 획득시
    public void RefreshItemSlotUI()
    {
        float firstTime = Time.time;

        // 소모품 개수 초기화
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

        // 소모품 UI 업데이트

        foreach(var kvp in consumableItems)
        {
            if(kvp.Value.count > 0)
            {
                CreateOrUpdateItemSlot(kvp.Value.item, true, kvp.Value.count);
            }
            else // 소모품이 0일 경우 아이템 삭제
            {
                RemoveItem(kvp.Value.item.ItemId);
            }
        }

        float time = Time.time - firstTime;

        Debug.Log($"걸린 시간 : {time}");
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

    // 장비 무기 아이템 삭제
    public void RemoveItem(int instanceId)
    {
        if (!itemSlotUI.ContainsKey(instanceId)) return;

        var item = itemSlotUI[instanceId];

        // item UI 슬롯 삭제
        Destroy(item.ItemSlot);

        var itemData = item.Item1.itemData;

        var itemList = allItem[(ItemType)itemData.Item_Type][(ItemTier)itemData.Item_Tier];

        itemList.Remove(item.Item1);

        // 소모품 이면 소모품 컨테이너도 에서도 삭제해 줘야함
    }

    // 소모품 아이템 삭제

}

public enum ItemType
{
    Weapon = 1, // 무기
    Helmet = 2, // 투구
    Armor = 3, // 갑옷
    Shose = 4, // 신발
    EquipmentGem = 5, // 장비 원석
    ReinforcedStone = 6, // 강화석
    Orb = 7, // 오브
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
