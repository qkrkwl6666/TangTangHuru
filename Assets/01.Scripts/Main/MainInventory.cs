using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using static UnityEditor.Progress;

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
    
    public PlayerViewUI playerViewUI;

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

        items.Add(710001);
        items.Add(710002);
        items.Add(710003);
        items.Add(710004);

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

        //�ӽ��ڵ�

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

    public Item MainInventoryAddItem(string itemId, int itemLevel = 0)
    {
        // ���� ���� ����

        var item = DataTableManager.Instance.Get<ItemTable>
            (DataTableManager.item).GetItemData(itemId).DeepCopy();

        if (item == null) return null;

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
            return mainItem;
        }

        // ������ Ÿ���� �ְ� ������ Ƽ� ���ٸ� 
        if (!allItem[itemType].ContainsKey(itemTier))
        {
            allItem[itemType].Add(itemTier, new List<Item>());

            allItem[itemType][itemTier].Add(mainItem);
            return mainItem;
        }

        // ������ Ÿ�԰� ������ Ƽ� �ִٸ�
        {
            allItem[itemType][itemTier].Add(mainItem);
            return mainItem;
        }
    }

    // ****����**** : Item �ν��Ͻ� ���� ���̵� �����
    public Item MainInventoryAddItem(Item item)
    {
        if (item == null) return null;

        var itemData = DataTableManager.Instance.Get<ItemTable>
            (DataTableManager.item).GetItemData(item.ItemId.ToString()).DeepCopy();

        itemData.CurrentUpgrade = item.itemData.CurrentUpgrade;

        ItemType itemType = item.ItemType;
        ItemTier itemTier = item.ItemTier;

        var mainItem = MakeItem(itemData, true, item.InstanceId);

        if (!allItem.ContainsKey(itemType))
        {
            allItem.Add(itemType, new Dictionary<ItemTier, List<Item>>());
            allItem[itemType].Add(itemTier, new List<Item>());

            allItem[itemType][itemTier].Add(mainItem);
            return mainItem;
        }

        // ������ Ÿ���� �ְ� ������ Ƽ� ���ٸ� 
        if (!allItem[itemType].ContainsKey(itemTier))
        {
            allItem[itemType].Add(itemTier, new List<Item>());

            allItem[itemType][itemTier].Add(mainItem);
            return mainItem;
        }

        // ������ Ÿ�԰� ������ Ƽ� �ִٸ�
        {
            allItem[itemType][itemTier].Add(mainItem);
            return mainItem;
        }
    }

    public void SaveInventory()
    {
        SaveManager.SaveDataV1.allItem.Clear();

        SaveManager.SaveDataV1.Gold = 0;
        SaveManager.SaveDataV1.Diamond = 0;
        SaveManager.SaveDataV1.CurrentStage = 1;

        SaveManager.SaveDataV1.playerEquipment = playerEquipment.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Item1);

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

    public Item MakeItem(ItemData itemData, bool isInstanceId = false, int instanceId = 0)
    {
        switch (itemData.Item_Type)
        {
            case 1: // ����
            case 8: // �� Todo : �ӽ�
                {
                    M_Weapon m_weaponItem = new M_Weapon();

                    if(!isInstanceId)
                    {
                        instanceId = m_weaponItem.GetHashCode() + UnityEngine.Random.Range(1, 100000);
                    }

                    m_weaponItem.SetItemData(itemData, instanceId);

                    return m_weaponItem;
                }
            case 2: // ����
            case 3: // ����
            case 4: // �Ź�
                {
                    M_Armour m_armour = new M_Armour();

                    if (!isInstanceId)
                    {
                        instanceId = m_armour.GetHashCode() + UnityEngine.Random.Range(1, 100000);
                    }  

                    m_armour.SetItemData(itemData, instanceId);

                    return m_armour;
                }
            case 5: // ��� ����
            case 6: // ��ȭ��
                {
                    M_Item m_item = new M_Item();

                    if (!isInstanceId)
                    {
                        instanceId = m_item.GetHashCode() + UnityEngine.Random.Range(1, 100000);
                    }

                    m_item.SetItemData(itemData, instanceId);

                    return m_item;
                }

            case 7: // ����
                break;

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
                        case ItemType.Pet: // Todo �ӽ�
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

    public void RemoveItem(ItemType itemType, ItemTier itemTier, int removeCount)
    {

    }

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
            MainInventoryAddItem(item);
        }

        #region �ʱ� ������ ���� �׽�Ʈ �ڵ�
        if (items.Count == 0)
        {
            MainInventoryAddItem("200001", 0);
            MainInventoryAddItem("200101", 0);
            MainInventoryAddItem("210001", 0);
            MainInventoryAddItem("210101", 0);
            MainInventoryAddItem("220001", 0);
                                           
            MainInventoryAddItem("710001", 0);
            MainInventoryAddItem("710002", 0);
            MainInventoryAddItem("710003", 0);
            MainInventoryAddItem("710004", 0);
                                           
            //MainInventoryAddItem("400001", 0);
            //MainInventoryAddItem("400002", 0);
            //MainInventoryAddItem("400003", 0);
            //MainInventoryAddItem("400004", 0);
            //MainInventoryAddItem("400005", 0);
            //MainInventoryAddItem("401001", 0);
            //MainInventoryAddItem("401002", 0);
            //MainInventoryAddItem("401003", 0);
            //MainInventoryAddItem("401004", 0);
            //MainInventoryAddItem("401005", 0);
            //MainInventoryAddItem("402001", 0);
            //MainInventoryAddItem("402002", 0);
            //MainInventoryAddItem("402003", 0);
            //MainInventoryAddItem("402004", 0);
            //MainInventoryAddItem("402005", 0);
            //MainInventoryAddItem("400011", 0);
            //MainInventoryAddItem("400012", 0);
            //MainInventoryAddItem("400013", 0);
            //MainInventoryAddItem("400014", 0);
            //MainInventoryAddItem("400015", 0);
            //MainInventoryAddItem("401011", 0);
            //MainInventoryAddItem("401012", 0);
            //MainInventoryAddItem("401013", 0);
            //MainInventoryAddItem("401014", 0);
            //MainInventoryAddItem("401015", 0);
            //MainInventoryAddItem("402011", 0);
            //MainInventoryAddItem("402012", 0);
            //MainInventoryAddItem("402013", 0);
            //MainInventoryAddItem("402014", 0);
            //MainInventoryAddItem("402015", 0);
            //MainInventoryAddItem("400021", 0);
            //MainInventoryAddItem("400022", 0);
            //MainInventoryAddItem("400023", 0);
            //MainInventoryAddItem("400024", 0);
            //MainInventoryAddItem("400025", 0);
            //MainInventoryAddItem("401021", 0);
            //MainInventoryAddItem("401022", 0);
            //MainInventoryAddItem("401023", 0);
            //MainInventoryAddItem("401024", 0);
            //MainInventoryAddItem("401025", 0);
            //MainInventoryAddItem("402021", 0);
            //MainInventoryAddItem("402022", 0);
            //MainInventoryAddItem("402023", 0);
            //MainInventoryAddItem("402024", 0);
            //MainInventoryAddItem("402025", 0);
            //MainInventoryAddItem("400031", 0);
            //MainInventoryAddItem("400032", 0);
            //MainInventoryAddItem("400033", 0);
            //MainInventoryAddItem("400034", 0);
            //MainInventoryAddItem("400035", 0);
            //MainInventoryAddItem("401031", 0);
            //MainInventoryAddItem("401032", 0);
            //MainInventoryAddItem("401033", 0);
            //MainInventoryAddItem("401034", 0);
            //MainInventoryAddItem("401035", 0);
            //MainInventoryAddItem("402031", 0);
            //MainInventoryAddItem("402032", 0);
            //MainInventoryAddItem("402033", 0);
            //MainInventoryAddItem("402034", 0);
            //MainInventoryAddItem("402035", 0);
            //MainInventoryAddItem("400041", 0);
            //MainInventoryAddItem("400042", 0);
            //MainInventoryAddItem("400043", 0);
            //MainInventoryAddItem("400044", 0);
            //MainInventoryAddItem("400045", 0);
            //MainInventoryAddItem("401041", 0);
            //MainInventoryAddItem("401042", 0);
            //MainInventoryAddItem("401043", 0);
            //MainInventoryAddItem("401044", 0);
            //MainInventoryAddItem("401045", 0);
            //MainInventoryAddItem("402051", 0);
            //MainInventoryAddItem("402052", 0);
            //MainInventoryAddItem("402053", 0);
            //MainInventoryAddItem("402054", 0);
            //MainInventoryAddItem("402055", 0);
            //MainInventoryAddItem("400061", 0);
            //MainInventoryAddItem("400062", 0);
            //MainInventoryAddItem("400063", 0);
            //MainInventoryAddItem("400064", 0);
            //MainInventoryAddItem("400065", 0);
            //MainInventoryAddItem("401061", 0);
            //MainInventoryAddItem("401062", 0);
            //MainInventoryAddItem("401063", 0);
            //MainInventoryAddItem("401064", 0);
            //MainInventoryAddItem("401065", 0);
            //MainInventoryAddItem("402061", 0);
            //MainInventoryAddItem("402062", 0);
            //MainInventoryAddItem("402063", 0);
            //MainInventoryAddItem("402064", 0);
            //MainInventoryAddItem("402065", 0);
            //MainInventoryAddItem("400071", 0);
            //MainInventoryAddItem("400072", 0);
            //MainInventoryAddItem("400073", 0);
            //MainInventoryAddItem("400074", 0);
            //MainInventoryAddItem("400075", 0);
            //MainInventoryAddItem("401071", 0);
            //MainInventoryAddItem("401072", 0);
            //MainInventoryAddItem("401073", 0);
            //MainInventoryAddItem("401074", 0);
            //MainInventoryAddItem("401075", 0);
            //MainInventoryAddItem("402071", 0);
            //MainInventoryAddItem("402072", 0);
            //MainInventoryAddItem("402073", 0);
            //MainInventoryAddItem("402074", 0);
            //MainInventoryAddItem("402075", 0);
        }

        #endregion

        RefreshItemSlotUI();

        yield return new WaitForSeconds(0.3f);

        // �ε� �� ���׷��̵� 
        LoadWeaponUpgrade();

        RefreshItemSlotUI();

        yield return new WaitForSeconds(0.3f);

        // ���� ���� �����ϰ� �ִ� ������ �÷��̾ ���� ��Ű��
        LoadDataPlayerEquip();

        OnMainInventorySaveLoaded?.Invoke();
    }

    public IEnumerator SceneLoadMainInventory()
    {
        var items = SaveManager.SaveDataV1.allItem;

        foreach (var item in items)
        {
            MainInventoryAddItem(item);
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

    public void LoadWeaponUpgrade()
    {
        foreach (var itemSlot in itemSlotUI)
        {
            var item = itemSlot.Value.Item1;

            switch (item.itemData.Item_Type)
            {
                case (int)ItemType.Weapon:
                    var weaponItem = item as M_Weapon;

                    if (weaponItem == null) continue;

                    weaponItem.UpgradeWeapon(weaponItem.itemData.CurrentUpgrade);

                    break;

                case (int)ItemType.Helmet:
                case (int)ItemType.Armor:
                case (int)ItemType.Shose:
                    break;
            }
        }
    }

    public void LoadDataPlayerEquip()
    {
        var tempPlayerEquip = SaveManager.SaveDataV1.playerEquipment;

        foreach (var item in tempPlayerEquip)
        {
            if (itemSlotUI.ContainsKey(item.Value.InstanceId))
            {
                playerEquipment[item.Key] = (itemSlotUI[item.Value.InstanceId].Item1, itemSlotUI[item.Value.InstanceId].ItemSlot);
            }
        }

        foreach (var item in playerEquipment)
        {
            LoadEquipItem(item.Value.Item1);
        }
    }

    public void LoadEquipItem(Item item)
    {
        if (!itemSlotUI.TryGetValue(item.InstanceId, out var slot)) return;

        slot.ItemSlot.SetActive(false);

        if (item.ItemType == ItemType.Pet) return;

        defaultEquipmentSlotUI[(int)item.ItemType - 1].SetActive(false);

        equipmentSlotUI[(int)item.ItemType - 1].SetItemData(item, mainUI);
        equipmentSlotUI[(int)item.ItemType - 1].gameObject.SetActive(true);

        GameManager.Instance.playerEquipment = playerEquipment;

        RefreshCharacterSpine();
    }


    // ��� ����
    public void EquipItem(Item item)
    {
        if (!itemSlotUI.TryGetValue(item.InstanceId, out var slot)) return;

        // ���� ��� ���� ���̶�� ���� ����

        // Todo : �ӽ�
        if(ItemType.Pet == item.ItemType)
        {
            playerEquipment[(PlayerEquipment)item.itemData.Item_Type] = (item, slot.ItemSlot);
            GameManager.Instance.playerEquipment = playerEquipment;
            return;
        }

        if(playerEquipment.ContainsKey((PlayerEquipment)item.ItemType))
        {
            UnequipItem(playerEquipment[(PlayerEquipment)item.ItemType].Item1);
        }

        slot.ItemSlot.SetActive(false);

        defaultEquipmentSlotUI[item.itemData.Item_Type - 1].SetActive(false);

        equipmentSlotUI[item.itemData.Item_Type - 1].SetItemData(item, mainUI);
        equipmentSlotUI[item.itemData.Item_Type - 1].gameObject.SetActive(true);

        playerEquipment[(PlayerEquipment)item.itemData.Item_Type] = (item, slot.ItemSlot);

        GameManager.Instance.playerEquipment = playerEquipment;

        RefreshCharacterSpine();
    }

    public void UnequipItem(Item item)
    {
        playerEquipment[(PlayerEquipment)item.itemData.Item_Type].ItemSlot.SetActive(true);

        playerEquipment.Remove((PlayerEquipment)item.itemData.Item_Type);

        defaultEquipmentSlotUI[item.itemData.Item_Type - 1].SetActive(true);
        EquipmentSlotUI[item.itemData.Item_Type - 1].SetActive(false);

        GameManager.Instance.playerEquipment = playerEquipment;

        RefreshCharacterSpine();
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

    public void RefreshCharacterSpine()
    {
        foreach (var itemType in playerEquipment)
        {
            if(itemType.Value.Item1 == null) continue;

            switch (itemType.Key) 
            {
                case PlayerEquipment.Weapon:
                    playerViewUI.SetWeaponSkin(itemType.Value.Item1.itemData.Spine_Id);
                    break;
            }
        }

        // �÷��̾� ���Ⱑ ���� ��� 
        if (!playerEquipment.ContainsKey(PlayerEquipment.Weapon))
        {
            playerViewUI.SetNoneWeaponCharacterSkin(playerViewUI.CurrentCharacterSkin);
        }
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
    Pet = 8, // ��
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
    other = 5,
    Pet = 8, // ��
}

public enum WeaponType
{
    Axe = 200001,
    Bow = 210001,
    Crossbow = 210101,
    Wand = 220001,
    Staff = 220101,

    Count = 5,
}
