using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class M_UISlot : MonoBehaviour
{
    public Item item;

    // Ƽ�� �̹��� ����
    private ItemTier currentTier = ItemTier.None;
    public Transform rankTransform;

    public GameObject textGameobject;

    public TextMeshProUGUI itemCountText;
    public Image itemIcon;
    public Image outline;
    public Image background;

    private bool isConsumable = false;

    public MainUI mainUI;

    public void SetItemData(Item item, MainUI mainUI, bool isPetEquipSlot = false, bool isConsumable = false)
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

        if (isConsumable) return;

        CreateRankImage(item.ItemTier);
    }

    public void SetItemData(ItemData itemData, bool isConsumable = false)
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

        if (isConsumable) return;

        CreateRankImage((ItemTier)itemData.Item_Tier);
    }

    public void SetItemDataConsumable(Item item, int itemCount)
    {
        SetItemData(item, mainUI, false, true);

        // �Ҹ�ǰ ���� ���� �ؽ�Ʈ ������Ʈ Ȱ��ȭ ���ְ� ���� �־��ֱ�
        textGameobject.SetActive(true);
        itemCountText.text = itemCount.ToString();
        isConsumable = true;
    }

    public void SetItemDataConsumable(ItemData itemData, int itemCount)
    {
        SetItemData(itemData, true);

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

        switch (item.itemData.Item_Type)
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
                mainUI.SetEquipPetData(item);
                mainUI.SetActivePetPopUpUI(true);
                break;

            case (int)ItemType.EquipmentGem: // ��� ����
            case (int)ItemType.ReinforcedStone: // ��� ����
            case (int)ItemType.OrbAttack: // ��� ����
            case (int)ItemType.OrbDefence: // ��� ����
            case (int)ItemType.OrbHp: // ��� ����
            case (int)ItemType.OrbDodge: // ��� ����
            case (int)ItemType.PetFood: // ��� ����
                mainUI.SetConsumablePopUpData(item);
                mainUI.SetActiveConsumablePopUpUI(true);
                break;
        }
    }

    public void CreateRankImage(ItemTier itemTier)
    {
        if (currentTier != ItemTier.None && currentTier == itemTier) return;

        foreach (Transform t in rankTransform)
        {
            Destroy(t.gameObject);
        }

        currentTier = itemTier;

        switch (itemTier)
        {
            case ItemTier.Normal:
                Addressables.InstantiateAsync(Defines.cRank, rankTransform);
                break;
            case ItemTier.Rare:
                Addressables.InstantiateAsync(Defines.bRank, rankTransform);
                break;
            case ItemTier.Epic:
                Addressables.InstantiateAsync(Defines.aRank, rankTransform);
                break;
            case ItemTier.Unique:
                Addressables.InstantiateAsync(Defines.sRank, rankTransform);
                break;
            case ItemTier.Legendary:
                Addressables.InstantiateAsync(Defines.ssRank, rankTransform);
                break;
        }
    }

    // ���� ���� �˾�
    public void OnWeaponSlotButton()
    {
        mainUI.SetUnequipPopData(item);
        mainUI.SetActiveEquipPopUpUI(true);
    }
    // �� ���� �˾�
    public void OnPetSlotButton()
    {
        mainUI.SetUnequipPetData(item);
        mainUI.SetActivePetPopUpUI(true);
    }

    // ��� ���� ����
}
