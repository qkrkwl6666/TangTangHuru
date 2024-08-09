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

    public void SetItemData(Item item, MainUI mainUI)
    {
        this.item = item;

        this.mainUI = mainUI;

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
        switch(item.itemData.Outline)
        {
            case "Outline_Blue":
                background.color = Defines.blueColor;
                break;
            case "Outline_Green":
                background.color = Defines.greenColor;
                break;
            case "Outline_Orange":
                background.color = Defines.orangeColor;
                break;
            case "Outline_Purple":
                background.color = Defines.purpleColor;
                break;
            case "Outline_Red":
                background.color = Defines.redColor;
                break;
            case "Outline_White":
                background.color = Defines.whiteColor;
                break;
            case "Outline_Yellow":
                background.color = Defines.yellowColor;
                break;
        }
    }

    public void SetItemDataConsumable(Item item, int itemCount)
    {
        SetItemData(item, mainUI);

        // �Ҹ�ǰ ���� ���� �ؽ�Ʈ ������Ʈ Ȱ��ȭ ���ְ� ���� �־��ֱ�
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
            case (int)ItemType.Weapon:
            case (int)ItemType.Pet: // Todo : �ӽ��Դϴ�.
                mainUI.SetEquipPopData(item);
                mainUI.SetActiveEquipPopUpUI(true);
                break;

            case (int)ItemType.Helmet:
            case (int)ItemType.Armor:
            case (int)ItemType.Shose:
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
}
