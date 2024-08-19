using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class M_UISlot : MonoBehaviour
{
    public Item item;

    public GameObject textGameobject;

    public TextMeshProUGUI itemCountText;
    public Image itemIcon;
    public Image outline;
    public Image background;

    private bool isConsumable = false;

    public MainUI mainUI;

    public void SetItemData(Item item, MainUI mainUI, bool isPetEquipSlot = false)
    {
        this.item = item;
        this.mainUI = mainUI;

        if (isPetEquipSlot) return;

        // UI ������ ������ �� ���缭 ����

        // ������ ������
        Addressables.LoadAssetAsync<Sprite>(item.itemData.Texture_Id).Completed += (texture) => 
        {
            itemIcon.sprite = texture.Result;
        };

        // �׵θ� ������
        Addressables.LoadAssetAsync<Sprite>(item.itemData.Outline).Completed += (texture) =>
        {
            outline.sprite = texture.Result;
        };

        // ��� ����
        background.color = Defines.GetColor(item.itemData.Outline);
    }

    public void SetItemData(ItemData itemData)
    {
        // ������ ������
        Addressables.LoadAssetAsync<Sprite>(itemData.Texture_Id).Completed += (texture) =>
        {
            itemIcon.sprite = texture.Result;
        };

        // �׵θ� ������
        Addressables.LoadAssetAsync<Sprite>(itemData.Outline).Completed += (texture) =>
        {
            outline.sprite = texture.Result;
        };

        // ��� ����
        background.color = Defines.GetColor(itemData.Outline);
    }

    public void SetItemDataConsumable(Item item, int itemCount)
    {
        SetItemData(item, mainUI);

        // �Ҹ�ǰ ���� ���� �ؽ�Ʈ ������Ʈ Ȱ��ȭ ���ְ� ���� �־��ֱ�
        textGameobject.SetActive(true);
        itemCountText.text = itemCount.ToString();
        isConsumable = true;
    }

    public void SetItemDataConsumable(ItemData itemData, int itemCount)
    {
        SetItemData(itemData);

        textGameobject.SetActive(true);
        itemCountText.text = itemCount.ToString();
        isConsumable = true;
    }

    public void SetEquipUpgradeUI(Item item)
    {
        textGameobject.SetActive(true);

        itemCountText.text = $"+{item.itemData.CurrentUpgrade}";
    }

    public void ItemSlotButton()
    {
        // ���� ������ Ÿ�Կ� �´� UI �˾� ���� ���� ������ ������ ���� �˾�����
        // �ѱ�� ���⼭ isConsumable �� ���� �˾� ���� �ٸ��� ����

        switch(item.itemData.Item_Type)
        {
            case (int)ItemType.Axe:
            case (int)ItemType.Sword:
            case (int)ItemType.Bow:
            case (int)ItemType.Crossbow:
            case (int)ItemType.Wand:
            case (int)ItemType.Staff:
                mainUI.SetEquipPopData(item);
                mainUI.SetActiveEquipPopUpUI(true);
                break;

            case (int)ItemType.Helmet:
            case (int)ItemType.Armor:
            case (int)ItemType.Shose:
                mainUI.SetEquipPopData(item);
                mainUI.SetActiveEquipPopUpUI(true);
                break;
            case (int)ItemType.Pet: //���˾�
                mainUI.SetEquipPopData(item);
                mainUI.SetActiveEquipPopUpUI(true);
                break;
        }
    }

    // ���� ���� �˾�
    public void OnWeaponSlotButton()
    {
        mainUI.SetUnequipPopData(item);
        mainUI.SetActiveEquipPopUpUI(true);
    }

    // ��� ���� ����
}
