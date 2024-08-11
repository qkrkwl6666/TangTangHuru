using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class EquipPopUp : MonoBehaviour
{
    public Item currentItem;

    // ��ư
    public Button EquipButton;     // ����
    public Button UnEquipButton;   // ���� ����
    public Button UpgradeButton;   // ��ȭ
    public Button TierUpButton;    // �±�

    public Button CencelButton;

    public TextMeshProUGUI titleText;

    // �±� UI
    public Slider tierUpSlider;
    public TextMeshProUGUI tierUpText;

    // ������ ������
    public Image itemImage;
    public Image bgImage;
    public Image outlineImage;

    // ������ ����
    public TextMeshProUGUI itemDescText;

    // ���׷��̵� 
    public TextMeshProUGUI needUpgradeGold;
    public TextMeshProUGUI totalUpgradeGold;

    public TextMeshProUGUI needReinforcedStone;
    public TextMeshProUGUI totalReinforcedStone;

    // ���� or ���� ���� �ٸ��� ǥ��

    public List<TextMeshProUGUI> itemStatusTexts = new List<TextMeshProUGUI>();

    public TextMeshProUGUI itemStatusText1; 
    public TextMeshProUGUI itemStatusText2;
    public TextMeshProUGUI itemStatusText3;
    public TextMeshProUGUI itemStatusText4;

    private MainInventory mainInventory;
    public MainUI mainUI;

    private void Start()
    {
        mainInventory = GameObject.FindWithTag("MainInventory").GetComponent<MainInventory>();  
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

        EquipButton.gameObject.SetActive(isEquip);
        UnEquipButton.gameObject.SetActive(!isEquip);

        UpgradeButton.interactable = !(currentItem.itemData.CurrentUpgrade >= 10);

        // ������ �̹���
        Addressables.LoadAssetAsync<Sprite>(item.itemData.Texture_Id).Completed += 
            (texture) =>
        {
            itemImage.sprite = texture.Result;
        };

        // ������ �ƿ����̳�
        Addressables.LoadAssetAsync<Sprite>(item.itemData.Outline).Completed +=
             (texture) =>
        {
            outlineImage.sprite = texture.Result;
        };

        // ������ ���

        bgImage.color = Defines.GetColor(item.itemData.Outline);

        titleText.text = DataTableManager.Instance.Get<StringTable>
            (DataTableManager.String).Get(item.itemData.Name_Id.ToString()).Text + $" +{item.itemData.CurrentUpgrade}";

        itemDescText.text = DataTableManager.Instance.Get<StringTable>
            (DataTableManager.String).Get(item.itemData.Desc_Id.ToString()).Text;

        // ������ ���� �ؽ�Ʈ ����
        switch (item.itemData.Item_Type)
        {
            case (int)ItemType.Weapon:
                itemStatusText1.text = Defines.damage + item.itemData.Damage;
                itemStatusText2.text = Defines.attackCoolTime + item.itemData.CoolDown;
                itemStatusText3.text = Defines.criticalChance + item.itemData.CriticalChance + "%";
                itemStatusText4.text = Defines.criticalDamage + item.itemData.Criticaldam + "%";

                foreach (var text in itemStatusTexts)
                {
                    text.gameObject.SetActive(true);
                }

                break;
            case (int)ItemType.Helmet:
                itemStatusText1.text = Defines.defence + item.itemData.Defense;
                itemStatusTexts[0].gameObject.SetActive(true);
                break;
            case (int)ItemType.Armor:
                itemStatusText1.text = Defines.hp + item.itemData.Hp;
                itemStatusTexts[0].gameObject.SetActive(true);
                break;
            case (int)ItemType.Shose:
                itemStatusText1.text = Defines.dodge + item.itemData.Dodge;
                itemStatusTexts[0].gameObject.SetActive(true);
                break;
        }
    }

    private void Awake()
    {
        CencelButton.onClick.AddListener(OnCencelButton);
        UpgradeButton.onClick.AddListener(OnUpgradeButton);
    }

    public void OnCencelButton()
    {
        currentItem = null;

        mainUI.SetActiveEquipPopUpUI(false);
    }

    public void OnUpgradeButton()
    {
        if(currentItem == null) return;

        switch(currentItem.ItemType)
        {
            case ItemType.Weapon:
                var weaponItem = currentItem as M_Weapon;

                if(weaponItem == null) return;

                weaponItem.UpgradeWeapon(weaponItem.itemData.CurrentUpgrade + 1);

                Debug.Log("���� ���׷��̵�");
                break;

            case ItemType.Helmet:
            case ItemType.Armor:
            case ItemType.Shose:
                break;
        }

        SetItemUI(currentItem);
        mainInventory.RefreshItemSlotUI();
    }

    // ��� ���� ��ư
    public void OnEquipEquipmentButton()
    {
        mainInventory.EquipItem(currentItem);

        mainUI.SetActiveEquipPopUpUI(false);
    }

    // ��� ���� ����
    public void OnUnequipEquipmentButton()
    {
        mainInventory.UnequipItem(currentItem);

        mainUI.SetActiveEquipPopUpUI(false);
    }
}
