using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using System;
using Unity.VisualScripting;

public class TierUpPopUp : MonoBehaviour
{
    public MainInventory mainInventory;

    public TextMeshProUGUI weaponTitleText;

    private float CurrentTierUpValue;
    private float MaxTierUpValue;

    public Slider currentTierUpSlider;
    public Slider subTierUpSlider;
    public TextMeshProUGUI currentTierUpText;

    public Transform content;

    public Button tierUpButton;
    public Button cencelButton;

    public EquipPopUp equipPopUp;
    public AppraisalPopUp appraisalPopUp;

    public List<(Item, TierUpItemSlotUI)> tierUpItemSlot = new ();

    private bool isMaxValue = false;

    private Item item;

    private void Awake()
    {
        cencelButton.onClick.AddListener(OnCencelButton);
    }

    public void RefreshItemCreateSlotUI(ItemType itemType)
    {
        List<Item> items = new ();

        switch (itemType)
        {
            case ItemType.Axe:
            case ItemType.Sword:
            case ItemType.Bow:
            case ItemType.Crossbow:
            case ItemType.Wand:
            case ItemType.Staff:
                items = mainInventory.GetWeaponItems();
                break;

            case ItemType.Helmet:
            case ItemType.Armor:
            case ItemType.Shose:
                items = mainInventory.GetArmourItems(itemType);
                break;
        }

        foreach (var mitem in items)
        {
            if (mitem == item) continue;

            Addressables.InstantiateAsync(Defines.tierUpItemSlot, content)
                .Completed += (slot) =>
                {
                    var go = slot.Result;
                    var tierItemSlot = go.GetComponent<TierUpItemSlotUI>();
                    tierItemSlot.SetItemData(mitem, this);

                    tierUpItemSlot.Add((mitem, tierItemSlot));
                };
        }

    }

    public void OnDisable()
    {
        foreach(var item in tierUpItemSlot)
        {
            Destroy(item.Item2.gameObject);
        }

        tierUpItemSlot.Clear();
        CurrentTierUpValue = 0f;
        MaxTierUpValue = 0f;
        isMaxValue = false;
    }

    public void SetItemUI(Item item)
    {
        this.item = item;

        string name = DataTableManager.Instance.Get<StringTable>(DataTableManager.String).Get(item.itemData.Name_Id).Text;
        weaponTitleText.text = $"{name} +{item.itemData.CurrentUpgrade}";

        currentTierUpText.text = $"{item.CurrentTierUp / item.itemData.TierUp_NeedExp * 100}%";

        currentTierUpSlider.value = item.CurrentTierUp;
        CurrentTierUpValue = item.CurrentTierUp;
        MaxTierUpValue = item.itemData.TierUp_NeedExp;

        subTierUpSlider.value = CurrentTierUpValue;
        subTierUpSlider.maxValue = MaxTierUpValue;

        currentTierUpSlider.maxValue = MaxTierUpValue;

        RefreshItemCreateSlotUI(item.ItemType);

        gameObject.SetActive(true);
    }

    public void OnCencelButton()
    {
        gameObject.SetActive(false);
    }

    public bool OnSlotButton(Item item, bool isFoucs)
    {
        if (isFoucs && isMaxValue) return false;

        if (isFoucs) 
        {
            CurrentTierUpValue += item.itemData.TierUp_Exp;
        }
        else
        {
            CurrentTierUpValue -= item.itemData.TierUp_Exp;
            isMaxValue = false;
        }

        subTierUpSlider.value = CurrentTierUpValue;

        if (CurrentTierUpValue >= MaxTierUpValue) isMaxValue = true;

        return true;
    }

    public void OnTierUpButton()
    {
        List<Item> removeItem = new List<Item>();

        foreach(var item in tierUpItemSlot)
        {
            if(item.Item2.IsFocus)
            {
                removeItem.Add(item.Item1);
            }
        }

        if (removeItem.Count == 0) return;

        foreach (var item in removeItem) 
        {
            mainInventory.RemoveItem(item.InstanceId);
        }

        // 승급 성공 다음 등급 아이템 생성 팝업 띄우기
        if(CurrentTierUpValue >= MaxTierUpValue)
        {
            TierUpNewItemPopUp();
            mainInventory.RefreshItemSlotUI();
            gameObject.SetActive(false);
            equipPopUp.gameObject.SetActive(false);
            return;
        }

        // 현재 승급 수치 현재 장비에 반영
        item.CurrentTierUp = CurrentTierUpValue;

        equipPopUp.SetTierUpUI(item);
        mainInventory.RefreshItemSlotUI();
        gameObject.SetActive(false);
    }

    public void TierUpNewItemPopUp()
    {
        int itemId = item.itemData.Item_Id + 1;

        // 현재 아이템 삭제
        mainInventory.RemoveItem(item.InstanceId);
        

        var itemMain = mainInventory.MainInventoryAddItem(itemId.ToString());

        List<Item> newItems = new List<Item>();
        newItems.Add(itemMain);

        appraisalPopUp.SetPopUp(newItems);
    }
}
