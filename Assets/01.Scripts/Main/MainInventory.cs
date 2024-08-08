using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class MainInventory : MonoBehaviour
{
    // 전체 아이템 컨테이너
    private Dictionary<ItemType, Dictionary<ItemTier, List<Item>>> allItem = new ();

    // 플레이어가 가지고있는 아이템 컨테이너 장비 
    // Todo : 로드 시 playerEquipment 에 등록된 item은 itemslot에서 비활성화 해줘야함
    private Dictionary<PlayerEquipment, (Item, GameObject ItemSlot)> playerEquipment = new ();

    public List<TextMeshProUGUI> equipmentTextUI = new (); // 업그레이드 텍스트   접근시 (PlayerEquipment) - 1 (적용 안됨)
    public List<GameObject> defaultEquipmentSlotUI = new (); // 기본 UI           접근시 (PlayerEquipment) - 1
    public List<GameObject> EquipmentSlotUI = new (); // 실제 아이템 UI           접근시 (PlayerEquipment) - 1

    public List<M_UISlot> equipmentSlotUI = new ();

    // 현재 생성된 UI 슬롯 아이템
    private SortedList<int, (Item, GameObject ItemSlot)> itemSlotUI = new ();

    // 소모품 아이템 컨테이너
    private Dictionary<string, (Item item, int count)> consumableItems = new();

    public Transform content;

    // 아이템 랜덤 테스트 코드 
    public List<int> items = new ();

    private MainUI mainUI;

    // 무기 UI 
    public Image weaponOutlineImage;
    public Image weaponIconImage;
    public Image weaponBgImage;

    public Action OnMainInventorySaveLoaded;

    private void Awake()
    {
        //var ab = allItem[ItemType.Bow][TierType.Rare][0];

        mainUI = GameObject.FindWithTag("MainUI").GetComponent<MainUI>();

        //items.Add(200001);
        //items.Add(200002);
        //items.Add(200003);
        //items.Add(200004);
        //items.Add(200005);
        //items.Add(200101);
        //items.Add(200102);
        //items.Add(200103);
        //items.Add(200104);
        //items.Add(200105);
        //items.Add(210001);
        //items.Add(210002);
        //items.Add(210003);
        //items.Add(210004);
        items.Add(600001);
        items.Add(600002);
        items.Add(600003);
        items.Add(600004);
        items.Add(600005);
        //items.Add(600006);
    }

    private void Start()
    {
        if (!DataTableManager.Instance.isTableLoad)
            DataTableManager.Instance.OnAllTableLoaded += CoSaveDataLoadMainInventory;
        else
        {
            Debug.Log("세이브 이미 로드 완료됨");
            StartCoroutine(SceneLoadMainInventory());
        }
    }

    private void OnDestroy()
    {
        
    }   

    public void CoSaveDataLoadMainInventory()
    {
        StartCoroutine(SaveDataLoadMainInventory());
    }


    public void CreateEquipmentGem()
    {
        int random = UnityEngine.Random.Range(0, items.Count);

        MainInventoryAddItem(items[random].ToString(), 0);
    }

    public void MainInventoryAddItem(string itemId, int itemLevel = 0)
    {
        // 깊은 복사 저장
        var item = DataTableManager.Instance.Get<ItemTable>
            (DataTableManager.item).GetItemData(itemId).DeepCopy();

        if (item == null) return;

        item.CurrentUpgrade = itemLevel;

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

    public void SaveInventory()
    {
        SaveManager.SaveDataV1.allItem.Clear();

        SaveManager.SaveDataV1.Gold = 0;
        SaveManager.SaveDataV1.Diamond = 0;
        SaveManager.SaveDataV1.CurrentStage = 1;

        foreach (var itemTypeDir in allItem) 
        { 
            foreach(var itemTierDir in itemTypeDir.Value)
            {
                foreach(var item in itemTierDir.Value) 
                {
                    SaveManager.SaveDataV1.allItem.Add(item);
                }
            }
        }
    }

    public Item MakeItem(ItemData itemData)
    {
        switch (itemData.Item_Type)
        {
            case 1: // 무기
                {
                    M_Weapon m_weaponItem = new M_Weapon();

                    int instanceId = m_weaponItem.GetHashCode() + UnityEngine.Random.Range(1, 100000);

                    m_weaponItem.SetItemData(itemData, instanceId);

                    return m_weaponItem;
                }
            case 2: // 투구

                break;
            case 3: // 갑옷

                break;
            case 4: // 신발

                break;
            case 5: // 장비 원석
            case 6: // 강화석
                {
                    M_Item m_item = new M_Item();
                    int instanceId = m_item.GetHashCode() + UnityEngine.Random.Range(1, 100000);

                    m_item.SetItemData(itemData, instanceId);

                    return m_item;
                }

        }

        return null;
    }

    // 현재 가지고 있는 아이템 ui 에 생성 이미 생성된 아이템 ui라면 넘기기
    // 새로고침 타이밍은 게임에 처음 접속시 인 게임 종료 후 메인씬 이동 시
    // 아이템을 획득시
    public void RefreshItemSlotUI()
    {
        //float firstTime = Time.time;

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

        //float time = Time.time - firstTime;

        //Debug.Log($"걸린 시간 : {time}");
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

                    go.GetComponent<M_UISlot>().SetItemData(item, mainUI);
                    go.GetComponent<M_UISlot>().SetItemDataConsumable(item, itemCount);

                    itemSlotUI.Add(item.ItemId, (item, go));
                };
            }
            else
            {
                itemSlotUI[item.ItemId].ItemSlot.GetComponent<M_UISlot>().SetItemDataConsumable(item, itemCount);
            }

            return;
        }

        if(!itemSlotUI.ContainsKey(item.InstanceId))
        {
            Addressables.InstantiateAsync(Defines.itemSlot, content).Completed += (itemGo) => 
            {
                var go = itemGo.Result;

                go.GetComponent<M_UISlot>().SetItemData(item, mainUI);

                go.GetComponent<M_UISlot>().SetEquipUpgradeUI(item);

                itemSlotUI.Add(item.InstanceId, (item, go));
            };
        }
        else
        {
            itemSlotUI[item.InstanceId].ItemSlot.GetComponent<M_UISlot>().SetEquipUpgradeUI(item);
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

    public void SaveMainInventory()
    {
        SaveInventory();

        SaveManager.Instance.SaveGame(SaveManager.SaveDataV1);
    }


    public IEnumerator SaveDataLoadMainInventory()
    {
        var items = SaveManager.SaveDataV1.allItem;

        foreach (var item in items) 
        {
            MainInventoryAddItem(item.ItemId.ToString(), item.itemData.CurrentUpgrade);
        }

        RefreshItemSlotUI();

        yield return new WaitForSeconds(0.3f);

        foreach (var itemSlot in itemSlotUI)
        {
            var item = itemSlot.Value.Item1;

            switch (item.itemData.Item_Type)
            {
                case (int)ItemType.Weapon:
                    var weaponItem = item as M_Weapon;

                    if (weaponItem == null) yield break;

                    weaponItem.UpgradeWeapon(weaponItem.itemData.CurrentUpgrade);

                    break;

                case (int)ItemType.Helmet:
                case (int)ItemType.Armor:
                case (int)ItemType.Shose:
                    break;
            }
        }

        RefreshItemSlotUI();

        yield return new WaitForSeconds(0.3f);

        OnMainInventorySaveLoaded?.Invoke();
    }

    public IEnumerator SceneLoadMainInventory()
    {
        var items = SaveManager.SaveDataV1.allItem;

        foreach (var item in items)
        {
            MainInventoryAddItem(item.ItemId.ToString(), item.itemData.CurrentUpgrade);
        }

        RefreshItemSlotUI();

        yield return new WaitForSeconds(0.3f);

        foreach (var itemSlot in itemSlotUI)
        {
            var item = itemSlot.Value.Item1;

            switch (item.itemData.Item_Type)
            {
                case (int)ItemType.Weapon:
                    var weaponItem = item as M_Weapon;

                    if (weaponItem == null) yield break;

                    weaponItem.UpgradeWeapon(weaponItem.itemData.CurrentUpgrade);

                    break;

                case (int)ItemType.Helmet:
                case (int)ItemType.Armor:
                case (int)ItemType.Shose:
                    break;
            }
        }

        RefreshItemSlotUI();

        yield return new WaitForSeconds(0.3f);

        GameManager.Instance.InitSaveLoaded();
    }


    // 장비 장착
    public void EquipItem(Item item)
    {
        if (!itemSlotUI.TryGetValue(item.InstanceId, out var slot)) return;

        // 기존 장비 장착 중이라면 장착 해제

        if(playerEquipment.ContainsKey((PlayerEquipment)item.ItemType))
        {
            UnequipItem(playerEquipment[(PlayerEquipment)item.ItemType].Item1);
        }

        slot.ItemSlot.SetActive(false);

        defaultEquipmentSlotUI[item.itemData.Item_Type - 1].SetActive(false);

        equipmentSlotUI[item.itemData.Item_Type - 1].SetItemData(item, mainUI);
        equipmentSlotUI[item.itemData.Item_Type - 1].gameObject.SetActive(true);

        playerEquipment[(PlayerEquipment)item.itemData.Item_Type] = (item, slot.ItemSlot);
    }

    public void UnequipItem(Item item)
    {
        playerEquipment[(PlayerEquipment)item.itemData.Item_Type].ItemSlot.SetActive(true);

        playerEquipment.Remove((PlayerEquipment)item.itemData.Item_Type);

        defaultEquipmentSlotUI[item.itemData.Item_Type - 1].SetActive(true);
        EquipmentSlotUI[item.itemData.Item_Type - 1].SetActive(false);
    }

    public int GetItemCount(ItemType itemType, ItemTier itemTier)
    {
        if (allItem.ContainsKey(itemType))
        {
            if(allItem[itemType].ContainsKey(itemTier))
            {
                return allItem[itemType][itemTier].Count;
            }
        }

        return default;
    }

    public Dictionary<ItemTier, List<Item>> GetItemTypes(ItemType itemType)
    {
        if(!allItem.ContainsKey(itemType)) return null;

        return allItem[itemType];
    }

    public List<Item> GetItemTypesTier(ItemType itemType, ItemTier itemTier)
    {
        if (!allItem.ContainsKey(itemType)) return null;
        if (!allItem[itemType].ContainsKey(itemTier)) return null;

        return allItem[itemType][itemTier];
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
    Orb = 7, // 오브
}

public enum ItemTier
{
    Normal,
    Rare,
    Epic,
    Unique,
    Legendary,
    Count
}

public enum PlayerEquipment
{
    Weapon = 1, // 무기
    Helmet = 2, // 투구
    Armor = 3,  // 갑옷
    Shoes = 4,  // 신발
}
