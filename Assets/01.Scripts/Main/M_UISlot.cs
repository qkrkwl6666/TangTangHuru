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
    public Image Outline;
    public Image Background;

    private bool isConsumable = false;

    public MainUI mainUI;

    public void SetItemData(Item item, MainUI mainUI)
    {
        this.item = item;

        this.mainUI = mainUI;

        // UI 아이템 데이터 에 맞춰서 설정

        // 아이템 아이콘
        Addressables.LoadAssetAsync<Sprite>(item.itemData.Texture_Id).Completed += (texture) => 
        {
            itemIcon.sprite = texture.Result;
        };

        // 테두리 아이콘
        Addressables.LoadAssetAsync<Sprite>(item.itemData.Outline).Completed += (texture) =>
        {
            Outline.sprite = texture.Result;
        };

        // 배경 색깔
        switch(item.itemData.Outline)
        {
            case "Outline_Blue":
                Background.color = Defines.blueColor;
                break;
            case "Outline_Green":
                Background.color = Defines.greenColor;
                break;
            case "Outline_Orange":
                Background.color = Defines.orangeColor;
                break;
            case "Outline_Purple":
                Background.color = Defines.purpleColor;
                break;
            case "Outline_Red":
                Background.color = Defines.redColor;
                break;
            case "Outline_White":
                Background.color = Defines.whiteColor;
                break;
            case "Outline_Yellow":
                Background.color = Defines.yellowColor;
                break;
        }
    }

    public void SetItemDataConsumable(Item item, int itemCount)
    {
        SetItemData(item, mainUI);

        // 소모품 개수 새서 텍스트 오브젝트 활성화 해주고 개수 넣어주기
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

        switch(item.itemData.Item_Type)
        {
            case (int)ItemType.Weapon:
                mainUI.SetEquipPopData(item);
                mainUI.SetActiveEquipPopUpUI(true);
                break;
            case (int)ItemType.Helmet:
            case (int)ItemType.Armor:
            case (int)ItemType.Shose:

                break;
        }
    }
}
