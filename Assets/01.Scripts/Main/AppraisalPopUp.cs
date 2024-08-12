using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class AppraisalPopUp : MonoBehaviour
{
    public Button confirmButton;

    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI descText;

    // ������ ������
    public Image itemImage;
    public Image bgImage;
    public Image outlineImage;

    // ���� or ���� ���� �ٸ��� ǥ��
    public TextMeshProUGUI itemStatusText1;
    public TextMeshProUGUI itemStatusText2;
    public TextMeshProUGUI itemStatusText3;
    public TextMeshProUGUI itemStatusText4;

    private MainInventory mainInventory;
    public MainUI mainUI;

    private List<Item> currentItemList;
    private int currentItemIndex;
    private bool isProcessingItems;

    // Todo : ���� ���� ���Ե� ���߿� �߰�

    private void Start()
    {
        //mainInventory = GameObject.FindWithTag("MainInventory").GetComponent<MainInventory>();

        confirmButton.onClick.AddListener(OnConfirmButton);
    }

    private void Update()
    {
        // ��� ������ ó���� �������� Ȯ��
        if (isProcessingItems && currentItemIndex >= currentItemList.Count)
        {
            isProcessingItems = false;
            gameObject.SetActive(false);
        }
    }

    public void SetItemUI(Item item, bool isEquip = true)
    {
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

        weaponNameText.text = DataTableManager.Instance.Get<StringTable>
            (DataTableManager.String).Get(item.itemData.Name_Id.ToString()).Text + $" +{item.itemData.CurrentUpgrade}";

        descText.text = DataTableManager.Instance.Get<StringTable>
            (DataTableManager.String).Get(item.itemData.Desc_Id.ToString()).Text;

        // ������ ���� �ؽ�Ʈ ����
        switch (item.itemData.Item_Type)
        {
            case (int)ItemType.Axe:
            case (int)ItemType.Sword:
            case (int)ItemType.Bow:
            case (int)ItemType.Crossbow:
            case (int)ItemType.Wand:
            case (int)ItemType.Staff:
                itemStatusText1.text = Defines.damage + item.itemData.Damage;
                itemStatusText2.text = Defines.attackCoolTime + item.itemData.CoolDown;
                itemStatusText3.text = Defines.criticalChance + item.itemData.CriticalChance + "%";
                itemStatusText4.text = Defines.criticalDamage + item.itemData.Criticaldam + "%";

                break;
            case (int)ItemType.Helmet:
            case (int)ItemType.Armor:
            case (int)ItemType.Shose:

                break;
        }
    }

    public void SetPopUp(List<Item> itemList)
    {
        currentItemList = itemList;
        currentItemIndex = 0;
        isProcessingItems = true;
        ShowNextItem();
    }

    private void ShowNextItem()
    {
        if (currentItemIndex < currentItemList.Count)
        {
            SetItemUI(currentItemList[currentItemIndex]);
            gameObject.SetActive(true);
        }
        else
        {
            isProcessingItems = false;
            gameObject.SetActive(false);
        }
    }

    public void OnConfirmButton()
    {
        currentItemIndex++;
        ShowNextItem();
    }
}
