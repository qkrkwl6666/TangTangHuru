using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class M_UISlot : MonoBehaviour
{
    public Item item;

    // 티어 이미지 변수
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

        // UI 아이템 데이터 에 맞춰서 설정

        // 아이템 아이콘
        Addressables.LoadAssetAsync<Sprite>(item.itemData.Texture_Id).Completed += (texture) =>
        {
            itemIcon.sprite = texture.Result;
        };

        // 테두리 아이콘
        Addressables.LoadAssetAsync<Sprite>(item.itemData.Outline).Completed += (texture) =>
        {
            outline.sprite = texture.Result;
        };

        // 배경 색깔
        background.color = Defines.GetColor(item.itemData.Outline);

        if (isConsumable) return;

        CreateRankImage(item.ItemTier);
    }

    public void SetItemData(ItemData itemData, bool isConsumable = false)
    {
        // 아이템 아이콘
        Addressables.LoadAssetAsync<Sprite>(itemData.Texture_Id).Completed += (texture) =>
        {
            itemIcon.sprite = texture.Result;
        };

        // 테두리 아이콘
        Addressables.LoadAssetAsync<Sprite>(itemData.Outline).Completed += (texture) =>
        {
            outline.sprite = texture.Result;
        };

        // 배경 색깔
        background.color = Defines.GetColor(itemData.Outline);

        if (isConsumable) return;

        CreateRankImage((ItemTier)itemData.Item_Tier);
    }

    public void SetItemDataConsumable(Item item, int itemCount)
    {
        SetItemData(item, mainUI, false, true);

        // 소모품 개수 새서 텍스트 오브젝트 활성화 해주고 개수 넣어주기
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
        // 현재 아이템 타입에 맞는 UI 팝업 띄우고 현재 아이템 데이터 정보 팝업으로
        // 넘기기 여기서 isConsumable 에 따라서 팝업 정보 다르게 띄우기

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
            case (int)ItemType.Pet: //펫팝업
                mainUI.SetEquipPetData(item);
                mainUI.SetActivePetPopUpUI(true);
                break;

            case (int)ItemType.EquipmentGem: // 장비 원석
            case (int)ItemType.ReinforcedStone: // 장비 원석
            case (int)ItemType.OrbAttack: // 장비 원석
            case (int)ItemType.OrbDefence: // 장비 원석
            case (int)ItemType.OrbHp: // 장비 원석
            case (int)ItemType.OrbDodge: // 장비 원석
            case (int)ItemType.PetFood: // 장비 원석
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

    // 무기 해제 팝업
    public void OnWeaponSlotButton()
    {
        mainUI.SetUnequipPopData(item);
        mainUI.SetActiveEquipPopUpUI(true);
    }
    // 펫 해제 팝업
    public void OnPetSlotButton()
    {
        mainUI.SetUnequipPetData(item);
        mainUI.SetActivePetPopUpUI(true);
    }

    // 장비 해제 찹업
}
