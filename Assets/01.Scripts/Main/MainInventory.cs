using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class MainInventory : MonoBehaviour
{
    // ��ü ������ �����̳�
    private Dictionary<ItemType, Dictionary<ItemTier, List<Item>>> allItem = new ();

    // �÷��̾ �������ִ� ������ �����̳� ��� 
    // Todo : �ε� �� playerEquipment �� ��ϵ� item�� itemslot���� ��Ȱ��ȭ �������
    private Dictionary<PlayerEquipment, (Item, GameObject ItemSlot)> playerEquipment = new ();

    public List<TextMeshProUGUI> equipmentTextUI = new (); // ���׷��̵� �ؽ�Ʈ   ���ٽ� (PlayerEquipment) - 1 (���� �ȵ�)
    public List<GameObject> defaultEquipmentSlotUI = new (); // �⺻ UI           ���ٽ� (PlayerEquipment) - 1
    public List<GameObject> EquipmentSlotUI = new (); // ���� ������ UI           ���ٽ� (PlayerEquipment) - 1

    public List<M_UISlot> equipmentSlotUI = new ();

    // ���� ������ UI ���� ������
    private SortedList<int, (Item, GameObject ItemSlot)> itemSlotUI = new ();

    // �Ҹ�ǰ ������ �����̳�
    private Dictionary<string, (Item item, int count)> consumableItems = new();

    public Transform content;

    // ������ ���� �׽�Ʈ �ڵ� 
    public List<int> items = new ();

    private MainUI mainUI;

    // ���� UI 
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
            Debug.Log("���̺� �̹� �ε� �Ϸ��");
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
        // ���� ���� ����
        var item = DataTableManager.Instance.Get<ItemTable>
            (DataTableManager.item).GetItemData(itemId).DeepCopy();

        if (item == null) return;

        item.CurrentUpgrade = itemLevel;

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
            case 1: // ����
                {
                    M_Weapon m_weaponItem = new M_Weapon();

                    int instanceId = m_weaponItem.GetHashCode() + UnityEngine.Random.Range(1, 100000);

                    m_weaponItem.SetItemData(itemData, instanceId);

                    return m_weaponItem;
                }
            case 2: // ����

                break;
            case 3: // ����

                break;
            case 4: // �Ź�

                break;
            case 5: // ��� ����
            case 6: // ��ȭ��
                {
                    M_Item m_item = new M_Item();
                    int instanceId = m_item.GetHashCode() + UnityEngine.Random.Range(1, 100000);

                    m_item.SetItemData(itemData, instanceId);

                    return m_item;
                }

        }

        return null;
    }

    // ���� ������ �ִ� ������ ui �� ���� �̹� ������ ������ ui��� �ѱ��
    // ���ΰ�ħ Ÿ�̹��� ���ӿ� ó�� ���ӽ� �� ���� ���� �� ���ξ� �̵� ��
    // �������� ȹ���
    public void RefreshItemSlotUI()
    {
        //float firstTime = Time.time;

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

        //float time = Time.time - firstTime;

        //Debug.Log($"�ɸ� �ð� : {time}");
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


    // ��� ����
    public void EquipItem(Item item)
    {
        if (!itemSlotUI.TryGetValue(item.InstanceId, out var slot)) return;

        // ���� ��� ���� ���̶�� ���� ����

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
    Legendary,
    Count
}

public enum PlayerEquipment
{
    Weapon = 1, // ����
    Helmet = 2, // ����
    Armor = 3,  // ����
    Shoes = 4,  // �Ź�
}
