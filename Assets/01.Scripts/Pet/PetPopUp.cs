using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class PetPopUp : MonoBehaviour
{
    public Item currentItem;

    // 버튼
    public Button EquipButton;     // 장착
    public Button UnEquipButton;   // 장착 해제
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

        if (currentItem == null)
        {
            Debug.Log("currentItem is null");
            return;
        }

        foreach (var text in itemStatusTexts)
        {
            text.gameObject.SetActive(false);
        }

        if (item.ItemTier == ItemTier.Legendary) TierUpButton.interactable = false;
        else TierUpButton.interactable = true;

        EquipButton.transform.parent.gameObject.SetActive(isEquip);
        UnEquipButton.transform.parent.gameObject.SetActive(!isEquip);

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
            case (int)ItemType.Pet:
                TierUpButton.transform.parent.gameObject.SetActive(false);
                break;
        }

    }

    public void SetTierUpUI(Item item)
    {
        tierUpSlider.value = item.CurrentTierUp;
        tierUpText.text = $"{item.CurrentTierUp / item.itemData.TierUp_NeedExp * 100}%";
    }

    private void Awake()
    {
        CencelButton.onClick.AddListener(OnCencelButton);
        TierUpButton.onClick.AddListener(OnTierUpPopUpButton);
    }

    public void OnCencelButton()
    {
        currentItem = null;

        mainUI.SetActiveEquipPopUpUI(false);
    }

    // 장비 장착 버튼
    public void OnEquipEquipmentButton()
    {
        mainInventory.EquipItem(currentItem);

        mainUI.SetActivePetPopUpUI(false);
    }

    // 장비 장착 해제
    public void OnUnequipEquipmentButton()
    {
        mainInventory.UnequipItem(currentItem);

        mainUI.SetActivePetPopUpUI(false);
    }

    public void OnTierUpPopUpButton()
    {
        tierUpPopUp.SetItemUI(currentItem);
    }
}
