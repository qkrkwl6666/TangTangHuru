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

    // Todo : ���� ���� ���Ե� ���߿� �߰�

    private void Start()
    {
        mainInventory = GameObject.FindWithTag("MainInventory").GetComponent<MainInventory>();

        confirmButton.onClick.AddListener(OnConfirmButton);
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
            case (int)ItemType.Weapon:
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

    public void OnConfirmButton()
    {
        gameObject.SetActive(false);
    }
}
