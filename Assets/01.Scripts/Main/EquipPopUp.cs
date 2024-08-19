using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class EquipPopUp : MonoBehaviour
{
    public Item currentItem;

    // 버튼
    public Button EquipButton;     // 장착
    public Button UnEquipButton;   // 장착 해제
    public Button UpgradeButton;   // 강화
    public Button TierUpButton;    // 승급

    public Button CencelButton;

    public TextMeshProUGUI titleText;

    // 승급 UI
    public Slider tierUpSlider;
    public TextMeshProUGUI tierUpText;

    // 아이템 아이콘
    public Image itemImage;
    public Image bgImage;
    public Image outlineImage;

    // 아이템 설명
    public TextMeshProUGUI itemDescText;
    public TextMeshProUGUI damageTypeText;

    // 업그레이드 
    public TextMeshProUGUI needUpgradeGold;
    public TextMeshProUGUI totalUpgradeGold;

    public TextMeshProUGUI needReinforcedStone;
    public TextMeshProUGUI totalReinforcedStone;

    // 무기 or 방어구에 따라 다르게 표시

    public List<TextMeshProUGUI> itemStatusTexts = new List<TextMeshProUGUI>();

    public TextMeshProUGUI itemStatusText1; 
    public TextMeshProUGUI itemStatusText2;
    public TextMeshProUGUI itemStatusText3;
    public TextMeshProUGUI itemStatusText4;

    public MainInventory mainInventory;
    public MainUI mainUI;
    public TierUpPopUp tierUpPopUp;

    private void Start()
    {
        
    }

    public void SetItemUI(Item item, bool isEquip = true)
    {
        currentItem = item;

        if(currentItem == null)
        {
            Debug.Log("currentItem is null");
            return;
        }

        foreach(var text in itemStatusTexts)
        {
            text.gameObject.SetActive(false);
        }

        if (item.ItemTier == ItemTier.Legendary) TierUpButton.interactable = false;
        else TierUpButton.interactable = true;

        EquipButton.transform.parent.gameObject.SetActive(isEquip);
        UnEquipButton.transform.parent.gameObject.SetActive(!isEquip);

        UpgradeButton.interactable = !(currentItem.itemData.CurrentUpgrade >= 10);

        tierUpSlider.value = item.CurrentTierUp;
        tierUpSlider.maxValue = item.itemData.TierUp_NeedExp;
        tierUpText.text = $"{item.CurrentTierUp / item.itemData.TierUp_NeedExp * 100}%";

        // 아이템 이미지
        Addressables.LoadAssetAsync<Sprite>(item.itemData.Texture_Id).Completed += 
            (texture) =>
        {
            itemImage.sprite = texture.Result;
        };

        // 아이템 아웃라이너
        Addressables.LoadAssetAsync<Sprite>(item.itemData.Outline).Completed +=
             (texture) =>
        {
            outlineImage.sprite = texture.Result;
        };

        // 아이템 배경

        bgImage.color = Defines.GetColor(item.itemData.Outline);

        titleText.text = DataTableManager.Instance.Get<StringTable>
            (DataTableManager.String).Get(item.itemData.Name_Id.ToString()).Text + $" +{item.itemData.CurrentUpgrade}";

        itemDescText.text = DataTableManager.Instance.Get<StringTable>
            (DataTableManager.String).Get(item.itemData.Desc_Id.ToString()).Text;

        // 아이템 스텟 텍스트 설정
        switch (item.itemData.Item_Type)
        {
            case (int)ItemType.Axe:
            case (int)ItemType.Sword:
            case (int)ItemType.Bow:
            case (int)ItemType.Crossbow:
            case (int)ItemType.Wand:
            case (int)ItemType.Staff:
                itemStatusText1.text = Defines.damage + item.itemData.Damage;
                itemStatusText2.text = Defines.attackCoolTime + item.itemData.CoolDown;
                itemStatusText3.text = Defines.criticalChance + item.itemData.CriticalChance + "%";
                itemStatusText4.text = Defines.criticalDamage + item.itemData.Criticaldam + "%";

                foreach (var text in itemStatusTexts)
                {
                    text.gameObject.SetActive(true);
                }
                TierUpButton.transform.parent.gameObject.SetActive(true);
                UpgradeButton.transform.parent.gameObject.SetActive(true);
                break;
            case (int)ItemType.Helmet:
                itemStatusText1.text = Defines.defence + item.itemData.Defense;
                itemStatusTexts[0].gameObject.SetActive(true);
                TierUpButton.transform.parent.gameObject.SetActive(true);
                UpgradeButton.transform.parent.gameObject.SetActive(true);
                damageTypeText.gameObject.SetActive(false);
                break;
            case (int)ItemType.Armor:
                itemStatusText1.text = Defines.hp + item.itemData.Hp;
                itemStatusTexts[0].gameObject.SetActive(true);
                TierUpButton.transform.parent.gameObject.SetActive(true);
                UpgradeButton.transform.parent.gameObject.SetActive(true);
                damageTypeText.gameObject.SetActive(false);
                break;
            case (int)ItemType.Shose:
                itemStatusText1.text = Defines.dodge + item.itemData.Dodge;
                itemStatusTexts[0].gameObject.SetActive(true);
                TierUpButton.transform.parent.gameObject.SetActive(true);
                UpgradeButton.transform.parent.gameObject.SetActive(true);
                damageTypeText.gameObject.SetActive(false);
                break;
            case (int)ItemType.Pet:
                TierUpButton.transform.parent.gameObject.SetActive(false);
                UpgradeButton.transform.parent.gameObject.SetActive(false);
                damageTypeText.gameObject.SetActive(false);
                break;
        }

        switch (item.itemData.Item_Type)
        {
            case (int)ItemType.Axe:
            case (int)ItemType.Crossbow:
            case (int)ItemType.Staff:
                damageTypeText.gameObject.SetActive(true);
                damageTypeText.text = "파워 타입";
                break;
            case (int)ItemType.Sword:
            case (int)ItemType.Bow:
            case (int)ItemType.Wand:
                damageTypeText.gameObject.SetActive(true);
                damageTypeText.text = "스피드 타입";
                break;
        }

                // 강화 UI 설정
                RefreshUpgradeTextUI(item.itemData.CurrentUpgrade);
    }

    public void SetTierUpUI(Item item)
    {
        tierUpSlider.value = item.CurrentTierUp;
        tierUpText.text = $"{item.CurrentTierUp / item.itemData.TierUp_NeedExp * 100}%";
    }
        
    private void Awake()
    {
        CencelButton.onClick.AddListener(OnCencelButton);
        UpgradeButton.onClick.AddListener(OnUpgradeButton);
        TierUpButton.onClick.AddListener(OnTierUpPopUpButton);
    }

    public void OnCencelButton()
    {
        currentItem = null;

        mainUI.SetActiveEquipPopUpUI(false);
    }

    public void RefreshUpgradeTextUI(int currentUpgrade)
    {
        needUpgradeGold.text = $"{mainInventory.Gold} / ";

        totalUpgradeGold.text = $" {Defines.defaultUpgradeGold * (currentUpgrade + 1)}";

        int reinforcedStone = 0;

        var list = mainInventory.GetItemTypesTier(ItemType.ReinforcedStone, ItemTier.Normal);

        if (list != null)
        {
            reinforcedStone = list.Count;
        }
        //reinforcedStone
        needReinforcedStone.text = $"{reinforcedStone} /";
        totalReinforcedStone.text = $" {Defines.defaultUpgradeReinforcedStone}";
    }

    public void OnUpgradeButton()
    {
        if(currentItem == null) return;

        if(!mainInventory.CheckUpgrade(currentItem)) return;

        switch (currentItem.ItemType)
        {
            case ItemType.Axe:
            case ItemType.Sword:
            case ItemType.Bow:
            case ItemType.Crossbow:
            case ItemType.Wand:
            case ItemType.Staff:
                var weaponItem = currentItem as M_Weapon;

                if(weaponItem == null) return;

                mainInventory.ItemUpgrade(currentItem);
                weaponItem.UpgradeWeapon(weaponItem.itemData.CurrentUpgrade + 1);

                RefreshUpgradeTextUI(currentItem.itemData.CurrentUpgrade);
                mainInventory.RefreshGoldDiamondTextUI();
                break;

            case ItemType.Helmet:
            case ItemType.Armor:
            case ItemType.Shose:
                break;
        }

        SetItemUI(currentItem);
        mainInventory.RefreshItemSlotUI();
        mainInventory.SaveInventory();
    }

    // 장비 장착 버튼
    public void OnEquipEquipmentButton()
    {
        mainInventory.EquipItem(currentItem);

        mainUI.SetActiveEquipPopUpUI(false);
    }

    // 장비 장착 해제
    public void OnUnequipEquipmentButton()
    {
        mainInventory.UnequipItem(currentItem);

        mainUI.SetActiveEquipPopUpUI(false);
    }

    public void OnTierUpPopUpButton()
    {
        tierUpPopUp.SetItemUI(currentItem);
    }
}
