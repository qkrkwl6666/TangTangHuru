using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class TierUpPopUp : MonoBehaviour
{
    public MainInventory mainInventory;

    public TextMeshProUGUI weaponTitleText;

    public Slider currentTierUpSlider;
    public TextMeshProUGUI currentTierUpText;

    public Transform content;

    public Button tierUpButton;
    public Button cencelButton;

    public List<(Item, TierUpItemSlotUI)> tierUpItemSlot = new ();

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

        foreach (var item in items)
        {
            Addressables.InstantiateAsync(Defines.tierUpItemSlot, content)
                .Completed += (slot) =>
                {
                    var go = slot.Result;
                    var tierItemSlot = go.GetComponent<TierUpItemSlotUI>();
                    tierItemSlot.SetItemData(item);

                    tierUpItemSlot.Add((item, tierItemSlot));
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
    }

    public void SetItemUI(Item item)
    {
        string name = DataTableManager.Instance.Get<StringTable>(DataTableManager.String).Get(item.itemData.Name_Id).Text;
        weaponTitleText.text = $"{name} +{item.itemData.CurrentUpgrade}";

        currentTierUpText.text = item.CurrentTierUp.ToString();
        currentTierUpSlider.value = item.CurrentTierUp;

        RefreshItemCreateSlotUI(item.ItemType);

        gameObject.SetActive(true);
    }

    public void OnCencelButton()
    {
        gameObject.SetActive(false);
    }
}
