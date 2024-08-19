using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class PetPopUp : MonoBehaviour
{
    public Item currentItem;

    // ��ư
    public Button equipButton;     // ����
    public Button unEquipButton;   // ���� ����
    public Button feedButton;    // �����ֱ�

    public Button CencelButton;

    public TextMeshProUGUI titleText;

    // �� ����ġ UI
    public Slider petExpSlider;
    public TextMeshProUGUI petExpText;

    // ������ ������
    public Image itemImage;

    // ������ ����
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

        // ������ �̹���
        Addressables.LoadAssetAsync<Sprite>(item.itemData.Texture_Id).Completed +=
            (texture) =>
            {
                itemImage.sprite = texture.Result;
            };

        titleText.text = DataTableManager.Instance.Get<StringTable>
            (DataTableManager.String).Get(item.itemData.Name_Id.ToString()).Text;

        itemDescText.text = DataTableManager.Instance.Get<StringTable>
            (DataTableManager.String).Get(item.itemData.Desc_Id.ToString()).Text;

        //���̰� �ϳ��� ������ feed��ư Ŭ���Ұ�
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

    // ��� ���� ��ư
    public void OnEquipPetButton()
    {
        mainInventory.EquipItem(currentItem);

        mainUI.SetActivePetPopUpUI(false);
    }

    // ��� ���� ����
    public void OnUnequipPetButton()
    {
        mainInventory.UnequipItem(currentItem);

        mainUI.SetActivePetPopUpUI(false);
    }

    public void OnClickFeedButton()
    {


    }
}
