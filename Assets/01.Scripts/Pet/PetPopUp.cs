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
    public Button EquipButton;     // ����
    public Button UnEquipButton;   // ���� ����
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

    // ���� or ���� ���� �ٸ��� ǥ��

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

    // ��� ���� ��ư
    public void OnEquipEquipmentButton()
    {
        mainInventory.EquipItem(currentItem);

        mainUI.SetActivePetPopUpUI(false);
    }

    // ��� ���� ����
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
