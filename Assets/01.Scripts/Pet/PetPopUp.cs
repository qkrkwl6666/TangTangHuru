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
    public Button equipButton;     // 장착
    public Button unEquipButton;   // 장착 해제
    public Button feedButton;    // 먹이주기

    public Button CencelButton;

    public TextMeshProUGUI titleText;

    // 펫 경험치 UI
    public Slider petExpSlider;
    public TextMeshProUGUI petExpText;

    // 아이템 아이콘
    public Image itemImage;

    // 아이템 설명
    public TextMeshProUGUI itemDescText;

    public MainInventory mainInventory;
    public MainUI mainUI;

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

        equipButton.transform.parent.gameObject.SetActive(isEquip);
        unEquipButton.transform.parent.gameObject.SetActive(!isEquip);

        petExpSlider.value = item.CurrentTierUp;
        petExpSlider.maxValue = item.itemData.TierUp_NeedExp;
        petExpText.text = $"{item.CurrentTierUp / item.itemData.TierUp_NeedExp * 100}%";

        // 아이템 이미지
        Addressables.LoadAssetAsync<Sprite>(item.itemData.Texture_Id).Completed +=
            (texture) =>
            {
                itemImage.sprite = texture.Result;
            };

        titleText.text = DataTableManager.Instance.Get<StringTable>
            (DataTableManager.String).Get(item.itemData.Name_Id.ToString()).Text;

        itemDescText.text = DataTableManager.Instance.Get<StringTable>
            (DataTableManager.String).Get(item.itemData.Desc_Id.ToString()).Text;

        //먹이가 하나도 없으면 feed버튼 클릭불가
        feedButton.interactable = false;

    }

    public void SetTierUpUI(Item item)
    {
        petExpSlider.value = item.CurrentTierUp;
        petExpText.text = $"{item.CurrentTierUp / item.itemData.TierUp_NeedExp * 100}%";
    }

    private void Awake()
    {
        CencelButton.onClick.AddListener(OnCencelButton);
        feedButton.onClick.AddListener(OnClickFeedButton);
    }

    public void OnCencelButton()
    {
        currentItem = null;

        mainUI.SetActivePetPopUpUI(false);
    }

    // 장비 장착 버튼
    public void OnEquipPetButton()
    {
        mainInventory.EquipItem(currentItem);

        mainUI.SetActivePetPopUpUI(false);
    }

    // 장비 장착 해제
    public void OnUnequipPetButton()
    {
        mainInventory.UnequipItem(currentItem);

        mainUI.SetActivePetPopUpUI(false);
    }

    public void OnClickFeedButton()
    {


    }
}
