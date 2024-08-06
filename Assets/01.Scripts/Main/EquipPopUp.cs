using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class EquipPopUp : MonoBehaviour
{
    public ItemData itemData;

    // 버튼
    public Button EquipButton;   // 장착
    public Button UnEquip;   // 장착 해제
    public Button UpgradeButton; // 강화
    public Button TierUpButton; // 승급

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
    public TextMeshProUGUI itemStatusText1; 
    public TextMeshProUGUI itemStatusText2;
    public TextMeshProUGUI itemStatusText3;
    public TextMeshProUGUI itemStatusText4;

    public void SetItemUI(ItemData itemData)
    {
        // 아이템 이미지
        Addressables.LoadAssetAsync<Sprite>(itemData.Texture_Id).Completed += 
            (texture) =>
        {
            itemImage.sprite = texture.Result;
        };

        // 아이템 아웃라이너
        Addressables.LoadAssetAsync<Sprite>(itemData.Outline).Completed +=
             (texture) =>
        {
            outlineImage.sprite = texture.Result;
        };

        // 아이템 배경

        bgImage.color = Defines.GetColor(itemData.Outline);

        titleText.text = DataTableManager.Instance.Get<StringTable>
            (DataTableManager.String).Get(itemData.Name_Id.ToString()).Text;

        itemDescText.text = DataTableManager.Instance.Get<StringTable>
            (DataTableManager.String).Get(itemData.Desc_Id.ToString()).Text;

        // 아이템 스텟 텍스트 설정
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
