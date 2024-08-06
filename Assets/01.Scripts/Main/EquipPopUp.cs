using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class EquipPopUp : MonoBehaviour
{
    public ItemData itemData;

    // ��ư
    public Button EquipButton;   // ����
    public Button UnEquip;   // ���� ����
    public Button UpgradeButton; // ��ȭ
    public Button TierUpButton; // �±�

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
    public TextMeshProUGUI itemStatusText1; 
    public TextMeshProUGUI itemStatusText2;
    public TextMeshProUGUI itemStatusText3;
    public TextMeshProUGUI itemStatusText4;

    public void SetItemUI(ItemData itemData)
    {
        // ������ �̹���
        Addressables.LoadAssetAsync<Sprite>(itemData.Texture_Id).Completed += 
            (texture) =>
        {
            itemImage.sprite = texture.Result;
        };

        // ������ �ƿ����̳�
        Addressables.LoadAssetAsync<Sprite>(itemData.Outline).Completed +=
             (texture) =>
        {
            outlineImage.sprite = texture.Result;
        };

        // ������ ���

        bgImage.color = Defines.GetColor(itemData.Outline);

        titleText.text = DataTableManager.Instance.Get<StringTable>
            (DataTableManager.String).Get(itemData.Name_Id.ToString()).Text;

        itemDescText.text = DataTableManager.Instance.Get<StringTable>
            (DataTableManager.String).Get(itemData.Desc_Id.ToString()).Text;

        // ������ ���� �ؽ�Ʈ ����
        switch (itemData.Item_Type)
        {
            case (int)ItemType.Weapon:
                itemStatusText1.text = Defines.damage + itemData.Damage;
                itemStatusText2.text = Defines.attackCoolTime + itemData.CoolDown;
                itemStatusText3.text = Defines.criticalChance + itemData.Criticalper;
                itemStatusText3.text = Defines.criticalDamage + itemData.Criticaldam;

                break;
            case (int)ItemType.Helmet:
            case (int)ItemType.Armor:
            case (int)ItemType.Shose:

                break;
        }
    }

    private void Awake()
    {
        CencelButton.onClick.AddListener(OnCencelButton);
    }

    public void OnCencelButton()
    {
        gameObject.SetActive(false);
    }
}
